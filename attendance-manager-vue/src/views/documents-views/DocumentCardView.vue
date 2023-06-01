<template>
  <div>
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
    <div v-else-if="isTeacher">
      <!-- The tabs menu -->
      <v-tabs
        v-model="currentTab"
        background-color="transparent"
        color="black"
        centered
      >
        <v-tab> View created reports</v-tab>
        <v-tab> View reports when you are collaborator</v-tab>
      </v-tabs>

      <v-tabs-items v-model="currentTab" class="pa-3 remove-background-color">
        <!-- First tab: display created documents-->
        <v-tab-item>
          <ViewDocumentsListCardsComponent
            :documents="ownDocuments"
            v-if="ownDocuments.length != 0"
          />
          <MessageComponent 
          description="There are no reports that you create. If you want to create a involvement report, go to <a href='\\create-document'>Create new involvement report</a> page and complete the flow."
          fontWeight="bold"
          fontSize="20px"
          v-else/>
        </v-tab-item>
        <!-- Second tab: display documents where the user is collaborator-->
        <v-tab-item>
          <ViewDocumentsListCardsComponent
            :documents="collaboratorDocuments"
            v-if="collaboratorDocuments.length != 0"
          />
          <MessageComponent 
          description= "There are no involvement report where you are a collaborator."
          fontWeight="bold"
          fontSize="20px"
          v-else/>
        </v-tab-item>
      </v-tabs-items>
    </div>
    <div v-else>
      <ViewDocumentsListCardsComponent
        :documents="collaboratorDocuments"
        v-if="collaboratorDocuments.length != 0"
          />
          <MessageComponent 
          description="You are not member of any report yet. Only teachers can add you."
          fontWeight="bold"
          fontSize="20px"
          v-else/>
    </div>
  </div>
</template>

<style lang="css" scoped>
.remove-background-color {
  background-color: transparent;
}
</style>
<script lang="ts">
import { ReportCardViewModule } from "@/modules/document";
import ViewDocumentsListCardsComponent from "@/components/document-components/ViewDocumentsListCardsComponent.vue";
import Vue from "vue";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import ReportService from "@/services/report.service";
import { Toastification } from "@/plugins/vue-toastification";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";

export default Vue.extend({
  name: "DocumentCardView",
  components: {
    ViewDocumentsListCardsComponent,
    MessageComponent
},
  data: function () {
    return {
      /**
       * Variable used for saving the current tab:
       * 0 -> Created documents view
       * 1 -> Documents where the user is collaborator
       * */
      currentTab: 0,
      /** Use this boolean to display the progress circular component */
      documents: [] as ReportCardViewModule[],
      isLoading: true,
      // Add a flag to track if data fetching is successful
      isFetchSuccessful: false,
    };
  },
  computed: {
    /** Array of created documents */
    ownDocuments: function (): ReportCardViewModule[] {
      console.log(this.documents.filter((d) => d.isCreator));
      return this.documents.filter((d) => d.isCreator);
    },
    /** Array of documents where the used is collaborator */
    collaboratorDocuments: function (): ReportCardViewModule[] {
      console.log(this.documents.filter((d) => !d.isCreator));
      
      return this.documents.filter((d) => !d.isCreator);
    },
    /** Boolean value for determinate the current user role */
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
  },
  /** Load all the documents from the API */
  created: async function () {
    // Fetch data with a timeout of 30 seconds
    const fetchDataWithTimeout = async () => {
      try {
        this.documents = await ReportService.getReports();
        this.isFetchSuccessful = true;
      } catch (error) {
        Toastification.simpleError("An error occurred during data fetching.");
      } finally {
        this.isLoading = false;
      }
    };

    // Start fetching data
    fetchDataWithTimeout();

    // Set a timeout to hide the loader if data is not fetched
    setTimeout(() => {
      if (!this.isFetchSuccessful) {
        this.isLoading = false;
        Toastification.simpleError("Data fetching timeout");
      }
    }, 30000);
  },
});
</script>