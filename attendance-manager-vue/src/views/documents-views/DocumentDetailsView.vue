<template>
  <v-card>
    <v-toolbar class="black">
      <v-toolbar-title class="white--text">{{ documentInfo.title }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn @click="$router.go(-1)" dark icon>
        <v-icon>mdi-close</v-icon>
      </v-btn>
      <template v-slot:extension>
        <v-tabs v-model="tabs" dark centered>
          <v-tab v-for="n in ['Attendances', 'Total Attendances', 'About']" :key="n">
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
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  components: {
    AttendanceTimelineComponent,
    AboutDocumentComponent,
    TotalAttendancesComponent
  },
  data() {
    return {
      tabs: [],
    };
  },
  computed: {
    documentInfo: function(): DocumentFullViewModule {
      return storeHelper.documentStore.documentDetails;
    },
  },
  created: async function(){
    await storeHelper.documentStore.loadCurrentDocument(this.$route.params.id);
  },
});
</script>