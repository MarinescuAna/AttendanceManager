<template>
    <v-layout row>
      <v-flex v-for="card in documents" :key="card.documentId" md4 class="pa-3">
        <v-card>
          <v-img
            :src="require(`@/assets/images/${ImageSelector.selectClockImage(card.documentId)}`)"
            class="align-end"
            gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
            height="200px"
          >
            <v-card-title class="text-h5 black--text font-weight-bold">{{ card.title }}</v-card-title>
            <v-card-subtitle class="black--text"
            >{{ 'Course: ' + card.courseName }}</v-card-subtitle>
            <v-card-subtitle class="black--text"
            >{{ 'Specialization: ' + card.specializationName }}</v-card-subtitle>
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
</template>

<style scoped>
.button-color{
  background-color: black;
}
</style>

<script lang="ts">
import { DocumentViewModule } from "@/modules/document";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import {ImageSelector} from "@/shared/image";
export default Vue.extend({
  data(){
    return {
      ImageSelector
    }
  },
  computed: {
    documents(): DocumentViewModule[] {
      return storeHelper.documentStore.createdDocuments;
    },
  },
  async created() {
    await storeHelper.documentStore.loadCreatedDocuments();
  },
});
</script>