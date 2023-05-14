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
            :documents="documents"
            :message="emptyCreatedDocumentsMessage"
          />
        </v-tab-item>
        <!-- Second tab: display documents where the user is collaborator-->
        <v-tab-item>
          <ViewDocumentsListCardsComponent
            :documents="collaboratorDocuments"
            :message="emptyCollaboratorDocumentsMessage"
          />
        </v-tab-item>
      </v-tabs-items>
    </div>
    <div v-else>
      <ViewDocumentsListCardsComponent
        :documents="collaboratorDocuments"
        :message="emptyStudentDocumentsMessage"
      />
    </div>
    <v-speed-dial fixed absolute bottom right>
      <template v-slot:activator>
        <v-btn class="blue-grey lighten-2" @click="onReload" fab>
          <v-icon> mdi-cached </v-icon>
        </v-btn>
      </template>
    </v-speed-dial>
  </div>
</template>

<style lang="css" scoped>
.remove-background-color {
  background-color: transparent;
}
</style>
<script lang="ts">
import { DocumentViewModule } from "@/modules/document";
import ViewDocumentsListCardsComponent from "@/components/document-components/ViewDocumentsListCardsComponent.vue";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";

export default Vue.extend({
  name: "DocumentCardView",
  components: {
    ViewDocumentsListCardsComponent,
  },
  data: function () {
    return {
      /**
       * Variable used for saving the current tab:
       * 0 -> Created documents view
       * 1 -> Documents where the user is collaborator
       * */
      currentTab: 0,
      /** Message that should be displayed when the created documents list is empty */
      emptyCreatedDocumentsMessage:
        "There are no reports that you create. If you want to create a involvement report, go to <a href='\\create-document'>Create new involvement report</a> page and complete the flow.",
      /** Message that should be displayed when the documents list where the user is collaborator is empty */
      emptyCollaboratorDocumentsMessage:
        "There are no involvement report where you are a collaborator.",
      /** Message that should be displayed when the student has no documents */
      emptyStudentDocumentsMessage: "You are not member of any report yet.",
      /** Use this boolean to display the progress circular component */
      isLoading: true,
    };
  },
  computed: {
    /** Array of created documents */
    documents: function (): DocumentViewModule[] {
      return storeHelper.documentStore.documents.filter((d) => d.isCreator);
    },
    /** Array of documents where the used is collaborator */
    collaboratorDocuments: function (): DocumentViewModule[] {
      return storeHelper.documentStore.documents.filter((d) => !d.isCreator);
    },
    /** Boolean value for determinate the current user role */
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
  },
  /** Load all the documents from the API */
  created: async function () {
    await this._loadDocuments(false);
  },
  methods: {
    _loadDocuments: async function (reload: boolean): Promise<void> {
      this.isLoading = !(await storeHelper.documentStore.loadDocuments(reload));
    },
    onReload: async function (): Promise<void> {
      this._loadDocuments(true);
    },
  },
});
</script>