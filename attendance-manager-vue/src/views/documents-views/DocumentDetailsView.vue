<template>
  <v-card>
    <v-toolbar class="blue-grey lighten-4">
      <v-toolbar-title>{{ documentInfo.title }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn @click="$router.go(-1)" icon>
        <v-icon>mdi-exit-run</v-icon>
      </v-btn>
      <template v-slot:extension>
        <v-tabs v-model="tabs" centered>
          <v-tab v-for="n in ['Attendances', 'About']" :key="n">
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
        <AboutDocumentComponent />
      </v-tab-item>
    </v-tabs-items>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { DocumentFullViewModule } from "@/modules/document";
import AboutDocumentComponent from "@/components/document-components/AboutDocumentComponent.vue";
import AttendanceTimelineComponent from "@/components/document-components/AttendanceTimelineComponent.vue";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  components: {
    AttendanceTimelineComponent,
    AboutDocumentComponent,
  },
  data() {
    return {
      tabs: [],
    };
  },
  computed: {
    documentInfo(): DocumentFullViewModule {
      return storeHelper.documentStore.documentDetails;
    },
  },
  async created() {
    await storeHelper.documentStore.loadCurrentDocument(this.$route.params.id);
  },
});
</script>