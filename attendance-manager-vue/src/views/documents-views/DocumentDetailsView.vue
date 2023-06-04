<template>
  <div v-if="isLoading">
    <v-layout justify-center>
      <v-progress-circular
        :size="100"
        :width="8"
        color="black"
        show-arrows
        align-with-title
        indeterminate
      ></v-progress-circular>
    </v-layout>
  </div>
  <v-card v-else>
    <v-toolbar class="black">
      <v-toolbar-title class="white--text">{{
        documentInfo?.title
      }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn @click="$router.go(-1)" dark icon>
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <template v-slot:extension>
        <v-tabs v-model="selectedTab" dark centered>
          <v-tab v-for="n in tabs" :key="n">
            {{ n }}
          </v-tab>
        </v-tabs>
      </template>
    </v-toolbar>

    <v-tabs-items v-model="selectedTab" class="blue-grey lighten-5" touchless>
      <v-tab-item>
        <AttendanceTimelineComponent />
      </v-tab-item>
      <v-tab-item>
        <TotalInvolvementsComponent />
      </v-tab-item>
      <v-tab-item>
        <DocumentMembersComponent />
      </v-tab-item>
      <v-tab-item>
        <RewardsComponent />
      </v-tab-item>
      <v-tab-item v-if="isTeacher && !isMobile">
        <DocumentDashboardComponent />
      </v-tab-item>
      <v-tab-item v-if="isTeacher && isCreator">
        <SettingsDocumentComponent />
      </v-tab-item>
      <v-tab-item>
        <AboutDocumentComponent />
      </v-tab-item>
    </v-tabs-items>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { ReportViewModule } from "@/modules/document";
import AboutDocumentComponent from "@/components/document-components/tabs/AboutDocumentComponent.vue";
import AttendanceTimelineComponent from "@/components/document-components/tabs/AttendanceTimelineComponent.vue";
import DocumentMembersComponent from "@/components/document-components/tabs/DocumentMembersComponent.vue";
import SettingsDocumentComponent from "@/components/document-components/tabs/SettingsDocumentComponent.vue";
import DocumentDashboardComponent from "@/components/document-components/tabs/DocumentDashboardComponent.vue";
import RewardsComponent from "@/components/document-components/tabs/RewardsComponent.vue";
import storeHelper from "@/store/store-helper";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import TotalInvolvementsComponent from "@/components/document-components/tabs/TotalInvolvementsComponent.vue";

export default Vue.extend({
  name: "DocumentDetailsView",
  components: {
    AttendanceTimelineComponent,
    AboutDocumentComponent,
    DocumentMembersComponent,
    SettingsDocumentComponent,
    DocumentDashboardComponent,
    RewardsComponent,
    TotalInvolvementsComponent
},
  data: function () {
    return {
      /** Current selected tab */
      selectedTab: [],
      /** Use this boolean to display the progress circular component */
      isLoading: true,
      /** The current user is member */
      isCreator: true,
    };
  },
  computed: {
    /** Compute the array of tabs according to the current user role */
    tabs: function (): string[] {
      if (!this.isTeacher) {
        return [
          "Involvements",
          "Total involvements",
          "Peers",
          "Rewards",
          "About",
        ];
      }

      if (this.isTeacher && this.isCreator) {
        return this.isMobile? [
          "Involvements",
          "Total involvements",
          "Collaborators",
          "Rewards",
          "Settings",
          "About",
        ]:[
          "Involvements",
          "Total involvements",
          "Collaborators",
          "Rewards",
          "Statistics",
          "Settings",
          "About",
        ];
      }

      return this.isMobile? [
        "Involvements",
        "Total involvements",
        "Collaborators",
        "Rewards",
        "About",
      ]:[
        "Involvements",
        "Total involvements",
        "Collaborators",
        "Rewards",
        "Statistics",
        "About",
      ];
    },
    /** Get document details from the store to display the name or other data */
    documentInfo: function (): ReportViewModule {
      return storeHelper.documentStore.report;
    },
    /** Boolean value for determinate the current user role */
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
  },
  /**
   * Load the document details from the API
   *
   * REMARK:
   * Using double negation means that:
   * undefined, null, false etc => false
   * value => true
   * Because it's convert the value to boolean
   *
   * */
  created: async function () {
    if (typeof this.$route.params.id !== "undefined" || this.documentInfo) {
      const response = await storeHelper.documentStore.loadCurrentReport(
        this.$route.params.id
      );

      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      this.isCreator = storeHelper.documentStore.report.isCreator;
      this.isLoading = response ? false : response;
    }
  },
  /** Restore the store related to the current document when the component is destroyed, otherwise the data that appears for other
   * opened document  will be related  to the previously opened document */
  destroyed: function (): void {
    storeHelper.documentStore.resetCurrentDocumentStore();
  },
});
</script>