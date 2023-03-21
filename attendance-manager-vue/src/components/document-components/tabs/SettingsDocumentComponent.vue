<template>
  <v-layout class="pa-6" justify-center>
    <v-expansion-panels inset>
        <CollaboratorPanelComponent />
    </v-expansion-panels>
  </v-layout>
</template>
<script lang="ts">
import CollaboratorPanelComponent from "@/components/document-components/extention-panels/CollaboratorPanelComponent.vue"
import Vue from "vue";
export default Vue.extend({
  name: "SettingsDocumentComponent",
  components:{
    CollaboratorPanelComponent
  }
});
</script>