<template>
  <div v-if="!canGenerateDiagrams">
    <v-alert border="left" type="error" class="ma-4"
      >The diagrams can't be generated because you don't have enough data!</v-alert
    >
  </div>
  <div v-else-if="isLoading">
    <v-layout justify-center>
      <v-progress-circular
        :size="100"
        :width="8"
        color="black"
        indeterminate
      ></v-progress-circular>
    </v-layout>
  </div>
  <div v-else>
    <v-layout align-center column>
      <BarChartComponent
        :chartData="studentsInterestData"
        :xAxiesLabels="studentsInterestsLabels"
        title="Students interest regarding the course activity"
      />
      <LineChartComponent
        :chartData="attendancePercentagePerDay"
        :xAxiesLabels="attendancePercentageLabels"
        title="Attendance percentage for lessons"
      />
    </v-layout>
  </div>
</template>

<script lang="ts">
import storeHelper from "@/store/store-helper";
import BarChartComponent from "@/components/shared-components/charts/BarChartComponent.vue";
import LineChartComponent from "@/components/shared-components/charts/LineChartComponent.vue";
import Vue from "vue";

export default Vue.extend({
  name: "DocumentDashboardComponent",
  components: {
    BarChartComponent,
    LineChartComponent,
  },
  data: function () {
    return {
      /** Boolean for loading the data*/
      isLoading: true,
    };
  },
  computed: {
    /** Use this computed data to check if there are enough courses held to can generate the diagrams  */
    canGenerateDiagrams: function (): boolean {
      return storeHelper.documentStore.documentDetails.noLessons >= 3 /* && 
        storeHelper.documentStore.studentsTotalAttendances.filter(d=>d.isPresent).length > 5 */;
    },
    /**
     * Data related to students interest
     * NOTE:
     * Students interest is computed like this:
     *   - calculate a weighted average of the attendances and bonus points,
     *   where the attendances and bonus points are weighted by their relative importance (the fields requiered when the document is created)
     *   - Weighted_average=(% * attendance)+(% * bonus_points)
     *   - This average will be then divided by the total possible score of the course
     *   - Percentage = (weighted_average/total_possible_score) * 100
     */
    studentsInterestData: function (): object {
      return [
        {
          name: "Students interest regarding the lessons",
          data: storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
            (d) => d.lessonValue
          ),
        },
        {
          name: "Students interest regarding the laboratories",
          data: storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
            (d) => d.laboratoryValue
          ),
        },
        {
          name: "Students interest regarding the seminaries",
          data: storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
            (d) => d.seminaryValue
          ),
        },
      ];
    },
    studentsInterestsLabels: function (): string[] {
      return storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
        (d) => d.studentName
      );
    },
    /**
     * Compute the percentage of students that attend a course
     * NOTE:
     * Take the number of attendances for each day, divide by the number of students
     * that are supposed to attend that course on that day, and then multiply by 100 to get the percentage
     */
    attendancePercentagePerDay: function (): object {
      return [
        {
          name: "Attendance percentage per day for all activity",
          type: "column",
          data: storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage.map(
            (d) => d.percentage
          ),
        },
        {
          name: "Attendance percentage per day for lessons",
          type: "line",
          data: storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage
            .filter((d) => d.courseType === "Lesson")
            .map((d) => d.percentage),
        },
        {
          name: "Attendance percentage per day for laboratories",
          type: "line",
          data: storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage
            .filter((d) => d.courseType === "Laboratory")
            .map((d) => d.percentage),
        },
        {
          name: "Attendance percentage per day for seminaries",
          type: "line",
          data: storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage
            .filter((d) => d.courseType === "Seminary")
            .map((d) => d.percentage),
        },
      ];
    },
    attendancePercentageLabels: function (): string[] {
      return storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage.map(
        (d) => d.datetime
      );
    },
  },
  /** Load data related to the document dashboard before loading the DOM */
  beforeMount: async function (): Promise<void> {
    if (this.canGenerateDiagrams) {
      await storeHelper.documentStore.loadDocumentDashboard();
      this.isLoading = false;
    }
  },
});
</script>