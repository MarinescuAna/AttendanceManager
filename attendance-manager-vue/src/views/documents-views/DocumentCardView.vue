<template>
  <v-layout v-if="documents.length !== 0" row>
    <v-flex v-for="card in documents" :key="card.documentId" md4 class="pa-3">
      <v-card>
        <v-img
          :src="
            require(`@/assets/images/${ImageSelector.selectClockImage(
              card.documentId
            )}`)
          "
          class="align-end"
          gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
          height="200px"
        >
          <v-card-title class="text-h5 black--text font-weight-bold">{{
            card.title
          }}</v-card-title>
          <v-card-subtitle class="black--text">{{
            "Course: " + card.courseName
          }}</v-card-subtitle>
          <v-card-subtitle class="black--text">{{
            "Specialization: " + card.specializationName
          }}</v-card-subtitle>
        </v-img>
        <v-card-actions class="button-color">
          <v-row justify="center" class="pa-2">
            <v-btn
              dark
              icon
              :to="{ name: 'document', params: { id: card.documentId } }"
            >
              <v-icon>mdi-login-variant</v-icon>
            </v-btn></v-row
          >
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
  <v-layout v-else>
    <h1 v-html="message"></h1>
  </v-layout>
</template>

<style scoped>
.button-color {
  background-color: black;
}
</style>

<script lang="ts">
import { DocumentViewModule } from "@/modules/document";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import { ImageSelector } from "@/shared/image";
export default Vue.extend({
  data() {
    return {
      ImageSelector,
    };
  },
  computed: {
    documents: function (): DocumentViewModule[] {
      if (this.$route.name === "collaborator-documents") {
        return storeHelper.documentStore.collaboratorDocuments;
      } else {
        return storeHelper.documentStore.createdDocuments;
      }
    },
    message: function (): string {
      if (this.$route.name === "collaborator-documents") {
        return "There are no documents where you are a collaborator.";
      } else {
        return "There are no documents that you create. If you want to create a document, go to <a href='\\create-document'>Create new document</a> page and complete the flow.";
      }
    },
  },
  created: async function () {
    // load documents where the teacher is collaborator or created documents according to the route name
    if (this.$route.name === "collaborator-documents") {
      await storeHelper.documentStore.loadDocuments(false);
    } else {
      await storeHelper.documentStore.loadDocuments(true);
    }
  },
});
</script>