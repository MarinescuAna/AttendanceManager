<template>
  <v-card>
    <v-toolbar class="blue-grey lighten-4" max-height="60px">
      <v-btn icon @click="onCloseDialog">
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <v-toolbar-title>Course activities - {{attendanceCollectionDate}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <DotsMenuComponent />
    </v-toolbar>
    <v-card-text>
      <v-btn class="blue-grey lighten-2 ma-2" v-if="isTeacher" disabled>
        Upload course activities
      </v-btn>
      <v-btn
        class="blue-grey lighten-2 ma-2"
        @click="generateCodeDialog = true"
        v-if="isTeacher"
      >
        Generate code
      </v-btn>
      <v-btn
        class="blue-grey lighten-2 ma-2"
        @click="useCodeDialog = true"
        v-if="!isTeacher && !currentUserIsPresent"
      >
        Use code
      </v-btn>
      <v-btn
        class="blue-grey lighten-2 ma-2"
        title="Reload only the attendances, not the bonus points!"
        @click="onReloadAttendances"
      >
        <v-icon>mdi-cached</v-icon>
      </v-btn>

      <!--Attendance table-->
      <v-simple-table v-if="students.length !== 0">
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left">{{isTeacher? 'Name':'GDPR'}}</th>
              <th class="text-left">Last modified</th>
              <th class="text-left">Attendance</th>
              <th class="text-left">Bonus Points</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in students" :key="item.attendanceId">
              <td>{{ item.userId }}</td>
              <td>{{ item.updatedOn }}</td>
              <td>
                <v-checkbox
                  v-model="item.isPresent"
                  :disabled="!isTeacher"
                ></v-checkbox>
              </td>
              <td>
                <v-text-field
                  type="number"
                  style="width: 40px"
                  :disabled="!isTeacher"
                  v-model="item.bonusPoints"
                ></v-text-field>
              </td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
      <div v-else>
        <h3>
          There is no student here. Recreate the involvement report or reload the page.
        </h3>
      </div>
    </v-card-text>

    <v-dialog
      v-if="generateCodeDialog"
      v-model="generateCodeDialog"
      width="50%"
      :fullscreen="isMobile"
    >
      <GenerateInvolvementCodeDialog
        :attendanceCollectionId="attendanceCollectionId"
        @close="generateCodeDialog = false"
      />
    </v-dialog>
    <v-dialog
      v-if="useCodeDialog"
      v-model="useCodeDialog"
      width="50%"
      :fullscreen="isMobile"
    >
      <UseInvolvementCodeDialog
        :attendanceId="currentUserAttendanceId"
        :attendanceCollectionId="attendanceCollectionId"
        @close="useCodeDialog = false"
        @save="onUseGeneratedCode"
      />
    </v-dialog>
  </v-card>
</template>

<script lang="ts">
import {
  StudentAttendanceModule,
  StudentAttendanceInsertModule,
} from "@/modules/document/attendance";
import AttendanceService from "@/services/attendance.service";
import Vue from "vue";
import DotsMenuComponent from "@/components/shared-components/DotsMenuComponent.vue";
import GenerateInvolvementCodeDialog from "@/components/document-components/dialogs/GenerateInvolvementCodeDialog.vue";
import { Toastification } from "@/plugins/vue-toastification";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import UseInvolvementCodeDialog from "./UseInvolvementCodeDialog.vue";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  name: "AddAttendanceDialog",
  components: {
    DotsMenuComponent,
    GenerateInvolvementCodeDialog,
    UseInvolvementCodeDialog,
  },
  props: {
    /** The id of the selected attendance collection */
    attendanceCollectionId: Number,
    /** The date of the selected attendance collection */
    attendanceCollectionDate: String
  },
  data: function() {
    return {
      dialog2: false,
      dialog3: false,
      generateCodeDialog: false,
      initStudents: [] as StudentAttendanceModule[],
      students: [] as StudentAttendanceModule[],
      saveChanges: false,
      currentUserIsPresent: false,
      useCodeDialog: false,
      currentUserAttendanceId: 0,
    };
  },
  computed: {
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
  },
  created: async function (): Promise<void> {
    // get all the attendances
    this.students =
      await AttendanceService.getStudentsAttendancesByCollectionIdAsync(
        this.attendanceCollectionId
      );
    this.students.forEach((x) => this.initStudents.push(Object.assign({}, x)));

    // stop student to enter the code multiple times
    if (!this.isTeacher) {
      const currentUser = this.students.find(
        (s) => s.userId == AuthService.getDataFromToken()?.code
      );

      if (currentUser != null) {
        this.currentUserIsPresent = currentUser.isPresent;
        this.currentUserAttendanceId = currentUser.attendanceId;
      }
    }
  },
  methods: {
    /**
     * Reload only the attendances not the bouns points
     * @test This should be tested with students and generated code
     */
    onReloadAttendances: async function (): Promise<void> {
      let result =
        await AttendanceService.getStudentsAttendancesByCollectionIdAsync(
          this.attendanceCollectionId
        );

      result.forEach((element) => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        this.students.find(
          (x) => x.attendanceId == element.attendanceId
        )!.isPresent = element.isPresent;
      });
    },
    /**
     * Before closing the dialog, save the changes in case that are
     */
    onCloseDialog: async function (): Promise<void> {
      if (!this.isTeacher) {
        // only the teacher can update all the attendances
        this.$emit("close-dialog");
        return;
      }

      let studentsChanged: StudentAttendanceInsertModule[] = [];

      this.students.forEach((student) => {
        let oldStudent = this.initStudents.find(
          (x) => x.attendanceId === student.attendanceId
        );

        if (
          oldStudent?.bonusPoints !== student.bonusPoints ||
          oldStudent.isPresent !== student.isPresent
        ) {
          studentsChanged.push({
            attendanceID: student.attendanceId,
            bonusPoints: student.bonusPoints,
            isPresent: student.isPresent,
            userId: student.userId
          } as StudentAttendanceInsertModule);
        }
      });

      if (studentsChanged.length !== 0) {
        const response = await AttendanceService.addStudentsAttendances(
          {
            involvements: studentsChanged,
            reportId: storeHelper.documentStore.documentDetails.documentId
          }
        );

        if (!response) {
          Toastification.simpleError(
            "Something went wrong and not all the attendances was saved"
          );
        }
      }
      this.$emit("close-dialog");
    },
    /**
     * Update the attendances after the student use the code
     */
    onUseGeneratedCode: function (): void {
      this.currentUserIsPresent = true;
      this.useCodeDialog = false;
      this.students.forEach((student) => {
        if (student.attendanceId === this.currentUserAttendanceId) {
          student.isPresent = true;
        }
      });
    },
  },
});
</script>