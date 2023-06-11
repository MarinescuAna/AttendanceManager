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
      <StudentsInterestDiagramComponent :involvements="involvements"/>
      <AttendancePercentagePerDayDiagram :involvements="involvements"/>
      <TotalAttendancesDiagram :involvements="involvements"/>
    </v-layout>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import AttendancePercentagePerDayDiagram from "../diagrams/AttendancePercentagePerDayDiagram.vue";
import TotalAttendancesDiagram from "../diagrams/TotalAttendancesDiagram.vue";
import StudentsInterestDiagramComponent from "../diagrams/StudentsInterestDiagramComponent.vue";
import InvolvementService from "@/services/involvement.service";
import { InvolvementViewModule } from "@/modules/view-modules";

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
      this.involvements = await InvolvementService.getInvolvements(-1,'',false,false,true);
      this.isLoading = false;
  },
});
</script>