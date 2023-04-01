<template>
  <div>
    <v-layout align-center column>
      <BarChartComponent
        :chartData="studentsLessonInterest"
        :title="'Students interest regarding the course activity'"
      ></BarChartComponent>
    </v-layout>
  </div>
</template>

<script lang="ts">
import storeHelper from "@/store/store-helper";
import BarChartComponent from "@/components/shared-components/charts/BarChartComponent.vue";
import Vue from "vue";

export default Vue.extend({
  name: "DocumentDashboardComponent",
  components: {
    BarChartComponent,
  },
  computed: {
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
    studentsLessonInterest: function (): object {
      return {
        labels:
          storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
            (d) => d.studentName
          ),
        datasets: [
          {
            label: "Students interest regarding the lessons",
            data: storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
              (d) => d.lessonInterest
            ),
          },
          {
            label: "Students interest regarding the laboratoies",
            data: storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
              (d) => d.laboratoryInterest
            ),
          },
          {
            label: "Students interest regarding the seminaries",
            data: storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
              (d) => d.seminaryInterest
            ),
          },
        ],
      };
    },
  },
  /** Load data related to the document dashboard before loading the DOM */
  beforeMount: async function (): Promise<void> {
    await storeHelper.documentStore.loadDocumentDashboard();
  },
});
</script>