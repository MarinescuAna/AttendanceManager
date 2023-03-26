<template>
  <v-layout class="pa-6" justify-center column>
    <v-expansion-panels class="pa-1" inset>
      <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold"
          >Delete current document</v-expansion-panel-header
        >
        <v-expansion-panel-content>
          <v-layout justify-center class="ma-3">
            <v-btn color="error" width="100%" @click="onDeleteDocument"
              >Delete</v-btn
            >
          </v-layout>
        </v-expansion-panel-content>
      </v-expansion-panel>
    </v-expansion-panels>
    <v-expansion-panels class="pa-1" inset>
      <CollaboratorPanelComponent />
    </v-expansion-panels>
    <v-expansion-panels class="pa-1" inset>
      <UpdateDocumentInformationsComponent />
    </v-expansion-panels>
  </v-layout>
</template>
<script lang="ts">
import CollaboratorPanelComponent from "@/components/document-components/extention-panels/CollaboratorPanelComponent.vue";
import UpdateDocumentInformationsComponent from "@/components/document-components/extention-panels/UpdateDocumentInformationsComponent.vue";
import { Toastification } from "@/plugins/vue-toastification";
import storeHelper from "@/store/store-helper";
import Vue from "vue";

export default Vue.extend({
  name: "SettingsDocumentComponent",
  components: {
    CollaboratorPanelComponent,
    UpdateDocumentInformationsComponent,
  },
  methods: {
    /** Delete the document */
    onDeleteDocument: async function (): Promise<void> {
      if (
        confirm(
          "Are you sure that you want to delete this document? The process cannot be reverted!"
        )
      ) {
        const result = await storeHelper.documentStore.deleteDocument();

        if (result) {
          Toastification.success("The document was successfully deleted!");
          this.$router.push({ name: "documents" });
        }
      }
    },
  },
});
</script>