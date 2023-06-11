<template>
  <div>
    <v-layout align-center column>
      <v-flex class="d-flex flex-row ma-5" v-if="isTeacher">
        <v-autocomplete
          v-model="search"
          :items="fullnames"
          persistent-hint
          label="Search (student name)"
          maxlength="128"
          color="black"
          append-icon="mdi-magnify"
          style="width: 500px"
          class="mt-2"
          clearable
        >
        </v-autocomplete>
        <v-btn class="blue-grey lighten-2 ma-5" @click="loadInvolvements" icon>
          <v-icon>mdi-cached</v-icon>
        </v-btn>
      </v-flex>
      <v-flex class="mb-2" v-if="isTeacher">
        <v-data-table
          :headers="totalAttendancesHeader"
          :items="involvements"
          :search="search"
          :expanded.sync="expanded"
          show-expand
          single-expand
          item-key="userId"
          dense
          class="table-color"
        >
          <template v-slot:expanded-item="{ headers, item }">
            <td :colspan="headers.length" class="text-left black--text text-h6">
              <h1 class="fond-weight-bold ma-3">{{ item.userName }}</h1>
              <StudentAttendanceExpandedComponent
                :key="item.userId"
                :userId="item.userId"
              />
            </td>
          </template>
        </v-data-table>
      </v-flex>
      <v-flex class="mx-md-5 mb-2" v-else>
        <StudentAttendanceExpandedComponent :userId="currentUserId" />
      </v-flex>
    </v-layout>
  </div>
</template>

<style scoped>
.table-color {
  background-color: transparent;
}
</style>

<script lang="ts">
import Vue from "vue";
import { totalAttendancesHeader } from "@/components/document-components/TotalAttendancesHeader";
import StudentAttendanceExpandedComponent from "@/components/document-components/StudentAttendanceExpandedComponent.vue";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import InvolvementService from "@/services/involvement.service";
import { TotalInvolvementViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "TotalAttendancesComponent",
  components: {
    StudentAttendanceExpandedComponent,
  },
  data() {
    return {
      search: "",
      totalAttendancesHeader,
      // Elements that are currently expanded
      expanded: [],
      involvements: [] as TotalInvolvementViewModule[],
    };
  },
  created: async function (): Promise<void> {
    //if the current user is teacher, get the total of involvements per sutudent
    if (this.isTeacher) {
      await this.loadInvolvements();
    }
  },
  computed: {
    fullnames: function (): string[] {
      return this.involvements.map((x) => x.userName);
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.lg;
    },
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    currentUserId: function (): string {
      return AuthService.getDataFromToken()!.email;
    },
  },
  methods: {
    loadInvolvements: async function (): Promise<void> {
      this.involvements = await InvolvementService.getInvolvementsTotal();
    },
  },
});
</script>