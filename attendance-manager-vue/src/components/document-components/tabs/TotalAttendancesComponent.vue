<template>
  <div>
    <v-layout column>
      <v-flex class="mx-md-5 my-1" v-if="isTeacher">
        <v-autocomplete
          v-model="search"
          :items="fullnames"
          persistent-hint
          label="Search (name)"
          maxlength="128"
          color="black"
          append-icon="mdi-magnify"
        >
        </v-autocomplete>
      </v-flex>
      <v-flex class="mx-md-5 mb-2" v-if="isTeacher">
        <v-data-table
          :headers="totalAttendancesHeader"
          :items="attendances"
          :search="search"
          :expanded.sync="expanded"
          show-expand
          single-expand
          item-key="userID"
          dense
          class="elevation-1"
        >
          <template v-slot:expanded-item="{ headers, item }">
            <td :colspan="headers.length">
              <h1 class="fond-weight-bold ma-3">{{ item.userName }}</h1>
              <StudentAttendanceExpandedComponent :userId="item.userID" />
            </td>
          </template>
        </v-data-table>
      </v-flex>
      <v-flex class="mx-md-5 mb-2" v-if="!isTeacher">
            <StudentAttendanceExpandedComponent  :userId="currentUserId"/>
      </v-flex>
    </v-layout>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import { totalAttendancesHeader } from "@/components/document-components/TotalAttendancesHeader";
import { TotalAttendanceModule } from "@/modules/document/attendance";
import StudentAttendanceExpandedComponent from "@/components/document-components/StudentAttendanceExpandedComponent.vue";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";

export default Vue.extend({
  components: {
    StudentAttendanceExpandedComponent,
  },
  data() {
    return {
      search: "",
      totalAttendancesHeader,
      // Elements that are currently expanded
      expanded: [],
    };
  },
  computed: {
    fullnames: function (): string[] {
      return storeHelper.documentStore.documentDetails.totalAttendances.map((x) => x.userName);
    },
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    currentUserId: function(): string {
        return AuthService.getDataFromToken()!.email;
    },
    attendances: function(): TotalAttendanceModule[]{
        return storeHelper.documentStore.documentDetails.totalAttendances;
    }
  }
});
</script>