<template>
  <div>
    <v-layout>
      <v-flex class="pa-5">
        <v-data-table
          :headers="header"
          :items="members"
          item-key="email"
          dense
          class="table-color"
          v-if="members.length != 0"
        >
        </v-data-table>
        <div v-else>
          <MessageComponent description="There are no collaborators added yet. Go to the SETTINGS tab if you what to add a new collaborator." 
            fontSize="20px"
            fontWeight="bold"
            :color="WARNING_AMBER_DARKEN_4"
          />
        </div>
      </v-flex>
    </v-layout>
  </div>
</template>
<style scoped>
.table-color {
  background-color: transparent;
}
</style>
<script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import {WARNING_AMBER_DARKEN_4} from "@/shared/constants";
import { MembersDto } from "@/modules/view-modules";

export default Vue.extend({
    name: "DocumentMembersComponent",
    data: function() {
        return {
            WARNING_AMBER_DARKEN_4,
            header: [
                {
                    text: "Email",
                    value: "email",
                    class: "text-left black--text text-h6"
                },
                {
                    text: "Fullname",
                    value: "name",
                    class: "text-left black--text text-h6"
                },
            ],
        };
    },
    computed: {
        members: function (): MembersDto[] {
            return storeHelper.documentStore.report.members;
        },
    },
    components: { MessageComponent }
});
</script>