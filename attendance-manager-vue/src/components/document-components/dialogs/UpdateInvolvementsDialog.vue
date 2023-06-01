<template>
  <v-card>
    <v-toolbar class="blue-grey lighten-4" max-height="60px">
      <v-btn icon @click="onCloseDialog">
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <v-toolbar-title
        >Course activities - {{ attendanceCollectionDate }}</v-toolbar-title
      >
      <v-spacer></v-spacer>
      <DotsMenuComponent />
    </v-toolbar>
    <v-layout column>
      <v-btn-toggle class="ma-5" rounded>
        <v-btn class="blue-grey lighten-2" v-if="isTeacher" disabled>
          <v-icon v-if="isMobile">mdi-upload</v-icon>
          <div v-else>Upload course activities</div>
        </v-btn>
        <v-btn
          class="blue-grey lighten-2"
          @click="generateCodeDialog = true"
          title="Share this code with the students that are members of this report to let them enter their attendance."
          v-if="isTeacher"
        >
          <v-icon v-if="isMobile">mdi-lock</v-icon>
          <div v-else>Generate code</div>
        </v-btn>
        <v-btn
          class="blue-grey lighten-2"
          @click="useCodeDialog = true"
          title="Use the code provided by your teacher to confirm your attendance."
          :disabled="currentUserInvolvement?.isPresent"
          v-if="!isTeacher"
        >
          Use code
        </v-btn>
        <v-btn
          class="blue-grey lighten-2"
          @click="onSaveInvolvements"
          title="Save changes."
          :disabled="!saveChanges"
          v-if="isTeacher"
        >
          <v-icon>mdi-floppy</v-icon>
        </v-btn>
        <v-btn class="blue-grey lighten-2" @click="onReloadAttendances">
          <v-icon>mdi-cached</v-icon>
        </v-btn>
      </v-btn-toggle>

      <!--Attendance table-->
      <v-simple-table class="ma-2" v-if="involvements.length !== 0">
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left">{{ isTeacher ? "Name" : "GDPR" }}</th>
              <th class="text-left" v-if="isTeacher">{{ "Email" }}</th>
              <th class="text-left">Last modified</th>
              <th class="text-left">Attendance</th>
              <th class="text-left">Bonus Points</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in involvements" :key="item.involvementId">
              <td>{{ item.student }}</td>
              <td v-if="isTeacher">{{ item.email }}</td>
              <td>{{ item.updateOn }}</td>
              <td>
                <v-checkbox
                  v-model="item.isPresent"
                  :disabled="!isTeacher"
                  @change="onPresenceChanged(item)"
                ></v-checkbox>
              </td>
              <td>
                <v-text-field
                  type="number"
                  style="width: 40px"
                  :disabled="!isTeacher || !item.isPresent"
                  v-model="item.bonusPoints"
                ></v-text-field>
              </td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
      <div v-else>
        <MessageComponent
          description="There is no student here or the page is not loaded yet. Reload the page, this can be caused by an error."
          icon="mdi-alert"
          color="red"
          fontSize="24px"
          fontWeight="bold"
        />
      </div>
    </v-layout>

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
        :attendanceId="currentUserInvolvement?.involvementId"
        :attendanceCollectionId="attendanceCollectionId"
        @close="useCodeDialog = false"
        @save="onUseGeneratedCode"
      />
    </v-dialog>
  </v-card>
</template>

<script lang="ts">
import InvolvementService from "@/services/involvement.service";
import Vue from "vue";
import DotsMenuComponent from "@/components/shared-components/DotsMenuComponent.vue";
import GenerateInvolvementCodeDialog from "@/components/document-components/dialogs/GenerateInvolvementCodeDialog.vue";
import { Toastification } from "@/plugins/vue-toastification";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import UseInvolvementCodeDialog from "./UseInvolvementCodeDialog.vue";
import { InvolvementViewModule } from "@/modules/document/involvement";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";

export default Vue.extend({
  name: "AddAttendanceDialog",
  components: {
    DotsMenuComponent,
    GenerateInvolvementCodeDialog,
    UseInvolvementCodeDialog,
    MessageComponent,
  },
  props: {
    /** The id of the selected attendance collection */
    attendanceCollectionId: Number,
    /** The date of the selected attendance collection */
    attendanceCollectionDate: String,
  },
  data: function () {
    return {
      generateCodeDialog: false,
      involvements: [] as InvolvementViewModule[],
      involvementsCopy: [] as InvolvementViewModule[],
      useCodeDialog: false,
    };
  },
  computed: {
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    currentUserInvolvement: function (): InvolvementViewModule | undefined {
      return AuthService.getDataFromToken()?.role != Role[2]
        ? this.involvements.find(
            (s) => s.student == AuthService.getDataFromToken()?.code
          )
        : undefined;
    },
    saveChanges: function (): boolean {
      return InvolvementService.isChanged(
        this.involvementsCopy,
        this.involvements
      );
    },
  },
  created: async function (): Promise<void> {
    //get all the involvements
    this.onReloadAttendances();
  },
  methods: {
    /**If the student is not present, then the bonus points cannot be inserted */
    onPresenceChanged: function (item: InvolvementViewModule): void {
      if (!item.isPresent) {
        item.bonusPoints = 0;
      }
    },
    /**
     * Reload only the attendances and bonus points
     */
    onReloadAttendances: async function (): Promise<void> {
      if (this.involvements.length != 0) {
        this.involvements = [];
        this.involvementsCopy = [];
      }

      this.involvements = await InvolvementService.getInvolvements(
        this.attendanceCollectionId,
        "",
        !this.isTeacher,
        false,
        false
      );
      this.involvements.forEach((x) =>
        this.involvementsCopy.push(Object.assign({}, x))
      );
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

      if (this.saveChanges) {
        if (
          confirm(
            "You have some unsaved changes! If you go, the changes will be lost."
          )
        ) {
          this.$emit("close-dialog");
        }
      } else {
        this.$emit("close-dialog");
      }
    },
    onSaveInvolvements: async function (): Promise<void> {
      //get all the involvements that was updated
      const studentsChanged = InvolvementService.getInvolvementChanges(
        this.involvementsCopy,
        this.involvements
      );

      if (studentsChanged.length !== 0) {
        const response = await InvolvementService.addStudentsAttendances({
          involvements: studentsChanged,
        });

        if (!response) {
          Toastification.simpleError(
            "Something went wrong and not all the attendances was saved"
          );
        } else {
          this.involvementsCopy = this.involvements;
          Toastification.success(
            "All the involvements were successfully saved and the rewards were added."
          );
        }
      }
    },
    /**
     * Update the attendances after the student use the code
     */
    onUseGeneratedCode: function (): void {
      this.useCodeDialog = false;
      this.involvements.forEach((student) => {
        if (
          student.involvementId === this.currentUserInvolvement?.involvementId
        ) {
          student.isPresent = true;
        }
      });
      this.involvementsCopy.forEach((student) => {
        if (
          student.involvementId === this.currentUserInvolvement?.involvementId
        ) {
          student.isPresent = true;
        }
      });
    },
  },
});
</script>