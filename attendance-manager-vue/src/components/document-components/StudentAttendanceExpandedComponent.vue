<template>
  <v-layout column class="ma-3">
    <v-flex v-if="isTeacher">
      <v-btn
        color="black"
        class="white--text"
        :disabled="!saveChanges"
        @click="onSave"
      >
        Save Changes
      </v-btn>
    </v-flex>
    <v-layout wrap>
      <v-simple-table>
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left">Last Update</th>
              <th class="text-left">Attendance</th>
              <th class="text-left">Bonus Points</th>
              <th class="text-left">Course Type</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in userAttendances" :key="item.attendanceId">
              <td>{{ item.updatedOn }}</td>
              <td>
                <v-simple-checkbox
                  v-model="item.wasPresent"
                  @click="saveChanges = true"
                  :disabled="!isTeacher"
                ></v-simple-checkbox>
              </td>
              <td>
                <v-text-field
                  type="number"
                  v-model="item.bonusPoints"
                  @click="saveChanges = true"
                  :disabled="!isTeacher"
                ></v-text-field>
              </td>
              <td>{{ item.courseType }}</td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
      <v-simple-table>
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left">Course Type</th>
              <th class="text-left">Total attendances</th>
              <th class="text-left">Total points</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in resultsOverview" :key="item.courseType">
              <td>{{ item.courseType }}</td>
              <td>{{ item.attendances }}</td>
              <td>{{ item.bonusPoints }}</td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
    </v-layout>
  </v-layout>
</template>

<script lang="ts">
import Vue from "vue";
import {
  StudentAttendanceInsertModule,
  StudentAttendanceModule,
} from "@/modules/document/attendance";
import { studentAttendancesHeader } from "./TotalAttendancesHeader";
import AttendanceService from "@/services/attendance.service";
import storeHelper from "@/store/store-helper";
import { CourseType, Role } from "@/shared/enums";
import { Toastification } from "@/plugins/vue-toastification";
import AuthService from "@/services/auth.service";

interface ResultsOverview {
  courseType: string;
  attendances: number;
  bonusPoints: number;
}

export default Vue.extend({
  name: "StudentAttendanceExpandedComponent",
  props: {
    userId: String,
  },
  data: function () {
    return {
      studentAttendancesHeader,
      userAttendances: [] as StudentAttendanceModule[],
      resultsOverview: [] as ResultsOverview[],
      students: [] as StudentAttendanceModule[],
      initStudents: [] as StudentAttendanceModule[],
      saveChanges: false,
    };
  },
  computed: {
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
  },
  created: async function () {
    // load user attendances for the selected user in case that the current user's role is teacher,
    // or get from the store the attendances report for the current user
    this.userAttendances = this.isTeacher
      ? await AttendanceService.getStudentAttendancesByDocumentIdAndUserId(
          storeHelper.documentStore.documentDetails.documentId,
          this.userId
        )
      : await storeHelper.documentStore.documentDetails
          .currentStudentAttendances;

    // make a copy of the results
    this.userAttendances.forEach((x) =>
      this.initStudents.push(Object.assign({}, x))
    );

    // if there are attendances, make a total
    if (this.userAttendances.length != 0) {
      this._getResultsOverview();
    }
  },
  methods: {
    _getResultsOverview: function (): void {
      //get laboratories
      const laboratories = this.userAttendances.filter(
        (x) => x.courseType == CourseType[CourseType.Laboratory]
      );
      if (laboratories.length != 0) {
        this.resultsOverview.push({
          courseType: CourseType[CourseType.Laboratory],
          attendances: laboratories.filter((x) => x.wasPresent).length,
          bonusPoints: laboratories.reduce(
            (x, result) => x + result.bonusPoints,
            0
          ),
        } as ResultsOverview);
      }
      //get lessons
      const lessons = this.userAttendances.filter(
        (x) => x.courseType == CourseType[CourseType.Lesson]
      );
      if (lessons.length != 0) {
        this.resultsOverview.push({
          courseType: CourseType[CourseType.Lesson],
          attendances: lessons.filter((x) => x.wasPresent).length,
          bonusPoints: lessons.reduce((x, result) => x + result.bonusPoints, 0),
        } as ResultsOverview);
      }
      //get seminaries
      const seminaries = this.userAttendances.filter(
        (x) => x.courseType == CourseType[CourseType.Seminary]
      );
      if (seminaries.length != 0) {
        this.resultsOverview.push({
          courseType: CourseType[CourseType.Seminary],
          attendances: seminaries.filter((x) => x.wasPresent).length,
          bonusPoints: seminaries.reduce(
            (x, result) => x + result.bonusPoints,
            0
          ),
        } as ResultsOverview);
      }
    },
    onSave: async function (): Promise<void> {
      let studentsChanged: StudentAttendanceInsertModule[] = [];

      this.userAttendances.forEach((student) => {
        let oldStudent = this.initStudents.find(
          (x) => x.attendanceId === student.attendanceId
        );

        if (
          oldStudent?.bonusPoints !== student.bonusPoints ||
          oldStudent.isPresent !== student.wasPresent
        ) {
          studentsChanged.push({
            attendanceID: student.attendanceId,
            bonusPoints: student.bonusPoints,
            isPresent: student.wasPresent,
          } as StudentAttendanceInsertModule);
        }
      });

      if (studentsChanged.length !== 0) {
        const response = await AttendanceService.addStudentsAttendances(
          studentsChanged
        );

        if (!response) {
          Toastification.simpleError(
            "Something went wrong and not all the attendances was saved!"
          );
        }
      }
    },
  },
});
</script>