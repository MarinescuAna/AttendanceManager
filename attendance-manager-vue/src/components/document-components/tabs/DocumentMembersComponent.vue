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
            :color="'#FF6F00'"
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
import { DocumentMembersViewModule } from "@/modules/document";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";

export default Vue.extend({
    name: "DocumentMembersComponent",
    data() {
        return {
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
        members: function (): DocumentMembersViewModule[] {
            return storeHelper.documentStore.documentDetails.documentMembers;
        },
    },
    components: { MessageComponent }
});
</script>