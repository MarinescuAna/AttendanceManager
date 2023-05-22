<template>
  <div v-if="isLoading">
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
    <v-layout class="ma-3" align-center column>
      <StudentsInterestDiagramComponent />
      <AttendancePercentagePerDayDiagram/>
      <TotalAttendancesDiagram />
    </v-layout>
  </div>
</template>

<script lang="ts">
import storeHelper from "@/store/store-helper";
import { InvolvementViewModule } from "@/modules/document/involvement";
import Vue from "vue";
import AttendancePercentagePerDayDiagram from "../diagrams/AttendancePercentagePerDayDiagram.vue";
import TotalAttendancesDiagram from "../diagrams/TotalAttendancesDiagram.vue";
import StudentsInterestDiagramComponent from "../diagrams/StudentsInterestDiagramComponent.vue";

export default Vue.extend({
  name: "DocumentDashboardComponent",
  components: {
    AttendancePercentagePerDayDiagram,
    TotalAttendancesDiagram,
    StudentsInterestDiagramComponent
},
  data: function () {
    return {
      /** Boolean for loading the data*/
      isLoading: true,
      involvements: [] as InvolvementViewModule[],
    };
  },
  /** Load data related to the document dashboard before loading the DOM */
  beforeMount: async function (): Promise<void> {
      await storeHelper.involvementStore.loadInvolvements(false);
      this.isLoading = false;
  },
});
</script>