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
            <tr v-for="item in studentAttendances" :key="item.attendanceId">
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
            <tr>
              <td>Lesson</td>
              <td>{{ attendanceLessons }}</td>
              <td>{{ bonusPointsLessons }}</td>
            </tr>
            <tr>
              <td>Laboratory</td>
              <td>{{ attendanceLLaboratories }}</td>
              <td>{{ bonusPointsLaboratories }}</td>
            </tr>
            <tr>
              <td>Seminary</td>
              <td>{{ attendanceSeminaries }}</td>
              <td>{{ bonusPointsSeminaries }}</td>
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
    userId: {
      type: String
    }
  },
  data: function () {
    return {
      studentAttendancesHeader,
      resultsOverview: [] as ResultsOverview[],
      students: [] as StudentAttendanceModule[],
      initStudents: [] as StudentAttendanceModule[],
      saveChanges: false,
    };
  },
  computed: {
    /** Get the current user role */
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    /**
     * If the current user is teacher, filter all the attendances related to the selected user
     * otherwise just load the attendances for the current user
     */
    studentAttendances: function(): StudentAttendanceModule[] {
      return this.isTeacher? storeHelper.documentStore.studentsTotalAttendances.filter(u=> u.userId == this.userId)
          : storeHelper.documentStore.documentDetails.currentStudentAttendances;
    },
    attendanceLessons: function(): number{
      return this._getTotal(CourseType.Lesson,true);
    },
    bonusPointsLessons: function(): number{
      return this._getTotal(CourseType.Lesson,false);
    },
    attendanceLLaboratories: function(): number{
      return this._getTotal(CourseType.Laboratory,true);
    },
    bonusPointsLaboratories: function(): number{
      return this._getTotal(CourseType.Laboratory,false);
    },
    attendanceSeminaries: function(): number{
      return this._getTotal(CourseType.Seminary,true);
    },
    bonusPointsSeminaries: function(): number{
      return this._getTotal(CourseType.Seminary,false);
    }
  },
  created: async function () {
    // load user attendances for the selected user in case that the current user's role is teacher,
    // or get from the store the attendances report for the current user
    await storeHelper.documentStore.loadStudentTotalAttendances(this.isTeacher? this.userId: null);
    
    if(this.studentAttendances.length > 0){
      // make a copy of the results
      this.studentAttendances.forEach((x) =>
        this.initStudents.push(Object.assign({}, x))
      );
    }
  },
  methods: {
    _getTotal: function(type: CourseType, doAttendancesTotal: boolean): number {
      const courses = this.studentAttendances.filter(
        (x) => x.courseType == CourseType[type]
      );
      if (courses.length != 0) {
        return doAttendancesTotal? courses.filter((x) => x.wasPresent).length:
        courses.reduce((x, result) => x + result.bonusPoints, 0);
      }else{
        return 0;
      }
    },
    onSave: async function (): Promise<void> {
      let studentsChanged: StudentAttendanceInsertModule[] = [];

      this.studentAttendances.forEach((student) => {
        let oldStudent = this.initStudents.find(
          (x) => x.attendanceId === student.attendanceId
        );

        if (
          oldStudent?.bonusPoints !== student.bonusPoints ||
          oldStudent.wasPresent !== student.wasPresent
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