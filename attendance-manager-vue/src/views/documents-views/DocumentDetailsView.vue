<template>
  <v-card>
    <v-toolbar class="black">
      <v-toolbar-title class="white--text">{{
        documentInfo?.title
      }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn @click="$router.go(-1)" dark icon>
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <template v-slot:extension>
        <v-tabs v-model="tabs" dark centered>
          <v-tab v-for="n in isTeacher ? teacherTabs : studentTabs" :key="n">
            {{ n }}
          </v-tab>
        </v-tabs>
      </template>
    </v-toolbar>

    <v-tabs-items v-model="tabs">
      <v-tab-item>
        <AttendanceTimelineComponent />
      </v-tab-item>
      <v-tab-item>
        <TotalAttendancesComponent />
      </v-tab-item>
      <v-tab-item>
        <DocumentMembersComponent />
      </v-tab-item>
      <v-tab-item>
        <DocumentDashboardComponent />
      </v-tab-item>
      <v-tab-item v-if="isTeacher">
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
import { DocumentFullViewModule } from "@/modules/document";
import AboutDocumentComponent from "@/components/document-components/tabs/AboutDocumentComponent.vue";
import AttendanceTimelineComponent from "@/components/document-components/tabs/AttendanceTimelineComponent.vue";
import TotalAttendancesComponent from "@/components/document-components/tabs/TotalAttendancesComponent.vue";
import DocumentMembersComponent from "@/components/document-components/tabs/DocumentMembersComponent.vue";
import SettingsDocumentComponent from "@/components/document-components/tabs/SettingsDocumentComponent.vue";
import DocumentDashboardComponent from "@/components/document-components/tabs/DocumentDashboardComponent.vue";
import storeHelper from "@/store/store-helper";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";

export default Vue.extend({
  name: "DocumentDetailsView",
  components: {
    AttendanceTimelineComponent,
    AboutDocumentComponent,
    TotalAttendancesComponent,
    DocumentMembersComponent,
    SettingsDocumentComponent,
    DocumentDashboardComponent,
  },
  data: function () {
    return {
      /** Current selected tab */
      tabs: [],
      /** List of available tabs for teachers */
      teacherTabs: [
        "Attendances",
        "Total Attendances",
        "Members",
        "Dashboard",
        "Settings",
        "About",
      ],
      /** List of available tabs for students */
      studentTabs: [
        "Attendances",
        "Total Attendances",
        "Members",
        "Dashboard",
        "About",
      ],
    };
  },
  computed: {
    /** Get document details from the store to display the name or other data */
    documentInfo: function (): DocumentFullViewModule {
      return storeHelper.documentStore.documentDetails;
    },
    /** Boolean value for determinate the current user role */
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
  },
  /** Load the document details from the API */
  created: async function () {
    await storeHelper.documentStore.loadCurrentDocument(this.$route.params.id);
  },
  /** Restore the store related to the current document when the component is destroyed, otherwise the data that appears for other
   * opened document  will be related  to the previously opened document */
  destroyed: function (): void {
    storeHelper.documentStore.resetCurrentDocumentStore();
  },
});
</script>