<template>
  <v-layout class="pa-6" justify-center column>
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
import Vue from "vue";

export default Vue.extend({
  name: "SettingsDocumentComponent",
  components:{
    CollaboratorPanelComponent,
    UpdateDocumentInformationsComponent
  }
});
</script>