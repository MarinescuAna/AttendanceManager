<template>
    <v-container>
      <v-row justify="center">
        <v-col cols="12">
          <ManagementTableComponent :dataSource="attendances" :headers="headers" :title="'Courses'" :type="type" :expandDetails="false"/>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script lang="ts">
import Vue from "vue";
import ManagementTableComponent from "@/components/shared-components/ManagementTableComponent.vue";
import storeHelper from "@/store/store-helper";
import { TotalAttendancesHeader } from "@/components/shared-components/Headers";
import { ManagementDataType } from "@/shared/enums";
import {TotalAttendanceModule } from "@/modules/document/attendance";
import AttendanceService from "@/services/attendance.service";

export default Vue.extend({
  components: {
    ManagementTableComponent
  },
  data(){
    return {
      headers: TotalAttendancesHeader,
      type: ManagementDataType.TotalAttendance,
      attendances: [] as TotalAttendanceModule[]
    };
  },
  async created(){
    this.attendances = await AttendanceService.getTotalAttendancesBtDocumentId(storeHelper.documentStore.documentDetails.documentId);
  }
});
</script>