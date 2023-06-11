<template>
  <v-layout row wrap>
    <v-flex v-for="card in documents" :key="card.documentId" md4 class="pa-3">
      <v-card>
        <v-img
          :src="
            require(`@/assets/images/${ImageSelector.selectClockImage(
              card.documentId
            )}`)
          "
          gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
          height="150px"
        >
        </v-img>
        <v-card-title>
          <div class="text-h5 black--text font-weight-bold mb-2">
            {{ card.title }}
          </div>
        </v-card-title>
        <v-card-subtitle>
          <v-icon color="grey" left> mdi-clock </v-icon> Last update
          {{ getRelativeTime(card.updatedOn) }}</v-card-subtitle
        >
        <v-divider class="mx-6"></v-divider>
        <v-card-text>
          <v-chip class="ma-1">
            <v-icon color="red" left> mdi-book </v-icon>
            {{ card.courseName }}
          </v-chip>
          <v-chip class="ma-1">
            <v-icon color="brown" left> mdi-brightness-5 </v-icon>
            {{ card.specializationName }}
          </v-chip>
        </v-card-text>
        <v-card-actions>
          <v-row justify="center" class="mb-1">
            <v-btn
              class="dark_button white--text"
              :to="{ name: 'document', params: { id: card.documentId } }"
            >
              Open report
            </v-btn></v-row
          >
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
import { ImageSelector } from "@/shared/image";
import Vue from "vue";
import moment from "moment";
import { ReportCardViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "ReportCardsComponent",
  props: {
    /** Array of documents */
    documents: {
      type: Array as () => ReportCardViewModule[],
      required: true,
    },
  },
  data: function () {
    return {
      /** Additional class used for choosing an image for the background cards */
      ImageSelector,
    };
  },
  methods: {
    getRelativeTime(updateOn: string) {
      return moment(new Date(updateOn)).fromNow();
    },
  },
});
</script>