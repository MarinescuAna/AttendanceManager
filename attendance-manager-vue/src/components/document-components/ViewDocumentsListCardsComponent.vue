<template>
  <v-layout v-if="documents.length !== 0" row wrap>
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
          }} ({{ card.updatedOn }})</v-card-subtitle>
        </v-img>
        <v-card-actions class="button-color">
          <v-row justify="center" class="pa-2">
            <v-btn
              color="black"
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

<script lang="ts">
import { ReportViewModule } from "@/modules/document";
import { ImageSelector } from "@/shared/image";
import Vue from "vue";

export default Vue.extend({
  name: "ViewDocumentsListCardsComponent",
  props: {
    /** Array of documents */
    documents: {
      type: Array as () => ReportViewModule[],
      required: true,
      },
    /** Message that will be displayed in case that is no document */
    message: {
      type: String,
      required: true
    }
  },
  data: function () {
    return {
      /** Additional class used for choosing an image for the background cards */
      ImageSelector,
    };
  },
});
</script>