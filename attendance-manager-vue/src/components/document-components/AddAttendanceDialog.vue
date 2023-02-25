<template>
  <v-card>
    <v-toolbar class="blue-grey lighten-4" height="100">
      <v-btn icon @click="onCloseDialog()">
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <v-toolbar-title>Attendances</v-toolbar-title>
      <v-spacer></v-spacer>
      <DotsMenuComponent />
    </v-toolbar>
    <v-card-text>
      <v-btn
        class="blue-grey lighten-2 ma-2"
        @click="dialog2 = !dialog2"
        disabled
      >
        Upload attendance
      </v-btn>
      <v-btn class="blue-grey lighten-2 ma-2" @click="dialog3 = !dialog3">
        Generate code
      </v-btn>
      <v-btn
        class="blue-grey lighten-2 ma-2"
        title="Reload only the attendances, not the bonus points!"
        @click="onReloadAttendances"
      >
        <v-icon>mdi-cached</v-icon>
      </v-btn>

      <!--Attendance table-->
      <v-simple-table style="width: 40%" v-if="students.length !== 0">
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left">Name</th>
              <th class="text-left">Last modifyed</th>
              <th class="text-left">Attendance</th>
              <th class="text-left">Bonus Points</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in students" :key="item.attendanceID">
              <td>{{ item.userID }}</td>
              <td>{{ item.updateOn }}</td>
              <td>
                <v-checkbox v-model="item.isPresent"></v-checkbox>
              </td>
              <td>
                <v-text-field
                  type="number"
                  style="width: 40px"
                  v-model="item.bonusPoints"
                ></v-text-field>
              </td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
      <div v-else>
        <h3>
          There is no student here. Recreate the document or reload the page.
        </h3>
      </div>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import {
  StudentAttendanceModule,
  StudentAttendanceInsertModule,
} from "@/modules/document/attendance";
import AttendanceService from "@/services/attendance.service";
import Vue from "vue";
import DotsMenuComponent from "../shared-components/DotsMenuComponent.vue";

export default Vue.extend({
  components: {
    DotsMenuComponent,
  },
  props: {
    attendanceCollectionId: Number,
  },
  data() {
    return {
      dialog2: false,
      dialog3: false,
      initStudents: [] as StudentAttendanceModule[],
      students: [] as StudentAttendanceModule[],
      saveChanges: false,
    };
  },
  async created(): Promise<void> {
    this.students =
      await AttendanceService.getStudentsAttendancesByCollectionIdAsync(
        this.attendanceCollectionId
      );
    this.students.forEach((x) => this.initStudents.push(Object.assign({}, x)));
  },
  methods: {
    /**
     * Reload only the attendances not the bouns points
     * @test This should be tested with students and generated code
     */
    async onReloadAttendances(): Promise<void> {
      let result =
        await AttendanceService.getStudentsAttendancesByCollectionIdAsync(
          this.attendanceCollectionId
        );

      result.forEach((element) => {
        this.students.find(
          (x) => x.attendanceID == element.attendanceID
        )!.isPresent = element.isPresent;
      });
    },
    /**
     * Before closing the dialog, save the changes in case that are
     */
    async onCloseDialog(): Promise<void> {
      let studentsChanged: StudentAttendanceInsertModule[] = [];

      this.students.forEach((student) => {
        let oldStudent = this.initStudents.find(
          (x) => x.attendanceID === student.attendanceID
        );

        if (
          oldStudent?.bonusPoints !== student.bonusPoints ||
          oldStudent.isPresent !== student.isPresent
        ) {
          studentsChanged.push({
            attendanceID: student.attendanceID,
            bonusPoints: student.bonusPoints,
            isPresent: student.isPresent,
          } as StudentAttendanceInsertModule);
        }
      });
      console.log(studentsChanged);

      if (studentsChanged.length !== 0) {
        const response = await AttendanceService.addStudentsAttendances(
          studentsChanged
        );

        if (!response.isSuccess) {
          alert("Something went wrong and not all the attendances was saved");
        }
      }
      this.$emit("close-dialog");
    },
  },
});
</script>