<template>
  <div v-if="!canGenerateDiagrams">
    <v-alert border="left" type="error" class="ma-4"
      >The diagrams can't be generated because you don't have enough
      data!</v-alert
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
      <AttendancePercentagePerDayDiagram/>
      <TotalAttendancesDiagram />
    </v-layout>
  </div>
</template>

<script lang="ts">
import storeHelper from "@/store/store-helper";
import { InvolvementViewModule } from "@/modules/document/involvement";
import BarChartComponent from "@/components/shared-components/charts/BarChartComponent.vue";
import Vue from "vue";
import AttendancePercentagePerDayDiagram from "../diagrams/AttendancePercentagePerDayDiagram.vue";
import TotalAttendancesDiagram from "../diagrams/TotalAttendancesDiagram.vue";


export default Vue.extend({
  name: "DocumentDashboardComponent",
  components: {
    BarChartComponent,
    AttendancePercentagePerDayDiagram,
    TotalAttendancesDiagram
},
  data: function () {
    return {
      /** Boolean for loading the data*/
      isLoading: true,
      involvements: [] as InvolvementViewModule[],
    };
  },
  computed: {
    /** Use this computed data to check if there are enough courses held to can generate the diagrams  */
    canGenerateDiagrams: function (): boolean {
      return (
        storeHelper.documentStore.documentDetails.noLessons >= 3 /* && 
        storeHelper.documentStore.studentsTotalAttendances.filter(d=>d.isPresent).length > 5 */
      );
    },
    studentsInterestData: function (): object {
      let interest = storeHelper.documentStore.documentDetails.documentDashboard.studentInterests
      return [
        {
          name: "Students interest regarding the lessons",
          data: interest.map(
            (d) => d.lessonValue
          ),
        },
        {
          name: "Students interest regarding the laboratories",
          data: interest.map(
            (d) => d.laboratoryValue
          ),
        },
        {
          name: "Students interest regarding the seminaries",
          data: interest.map(
            (d) => d.seminaryValue
          ),
        },
      ];
    },
    studentsInterestsLabels: function (): string[] {
      return storeHelper.documentStore.documentDetails.documentDashboard.studentInterests.map(
        (d) => d.studentName
      );
    }
  },
  /** Load data related to the document dashboard before loading the DOM */
  beforeMount: async function (): Promise<void> {
    await storeHelper.involvementStore.loadInvolvements(false);
    if (this.canGenerateDiagrams) {
      await storeHelper.documentStore.loadDocumentDashboard();

      this.isLoading = false;
    }
  },
});
</script>