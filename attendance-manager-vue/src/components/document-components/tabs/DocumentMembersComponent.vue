<template>
  <div>
    <v-layout>
      <v-flex class="pa-5">
        <v-data-table
          :headers="header"
          :items="members"
          item-key="email"
          dense
          class="elevation-1"
        >
        </v-data-table>
      </v-flex>
    </v-layout>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import { DocumentMembersViewModule } from "@/modules/document";

export default Vue.extend({
  data() {
    return {
      header: [
        {
            text:"Email",
            value:'email'
        }, 
        {
            text:"Fullname",
            value:'name'
        }, 
        {
            text:"Role",
            value:'role'
        }, 
      ],
    };
  },
  computed: {
    members: function (): DocumentMembersViewModule[] {
      return storeHelper.documentStore.documentDetails.documentMembers;
    },
  },
});
</script>