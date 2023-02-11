<template>
  <v-container fluid>
    <v-row dense>
      <v-col v-for="card in documents" :key="card.documentId" cols="4">
        <v-card>
          <v-img
            src="https://cdn.vuetifyjs.com/images/cards/road.jpg"
            class="white--text align-end"
            gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
            height="200px"
          >
            <v-card-title v-text="card.title"></v-card-title>
            <v-card-subtitle
              v-text="'Course: ' + card.courseName"
            ></v-card-subtitle>
            <v-card-subtitle
              v-text="'Specialization: ' + card.specializationName"
            ></v-card-subtitle>
          </v-img>

          <v-card-actions>
            <v-row justify="center" class="pa-2">
              <v-btn
                icon
                :to="{ name: 'document', params: { id: card.documentId } }"
              >
                <v-icon>mdi-login-variant</v-icon>
              </v-btn></v-row
            >
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { DocumentViewModule } from "@/modules/document";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
export default Vue.extend({
  computed: {
    documents(): DocumentViewModule[] {
      return storeHelper.documentStore.createdDocuments;
    }
  },
  async created() {
    await storeHelper.documentStore.loadCreatedDocuments();
  },
});
</script>