<template>
    <div v-if="users.length != 0">
    <v-data-table
      :headers="headers"
      :items="users"
      expanded.sync
      sort-by="fullname"
      item-key="id"
      show-expand
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Users</v-toolbar-title>
        </v-toolbar>
      </template>
      <template v-slot:expanded-item="{ headers, item }">
        <td :colspan="headers.length">
          <UserInfoCardComponent
            :item="item"
          />
        </td>
      </template>
    </v-data-table>
  </div>
  <div v-else>
    <MessageComponent
            description="There is no student added."
            fontSize="20px"
            fontWeight="bold"
            icon="mdi-information"
            :color="WARNING_AMBER_DARKEN_4"
          />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import { UserViewModule } from "@/modules/view-modules";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import UserInfoCardComponent from "@/components/user/UserInfoCardComponent.vue";

export default Vue.extend({
    name: "UsersView",
    components: { UserInfoCardComponent },
    data: function () {
        return {
            WARNING_AMBER_DARKEN_4,
            headers: [
              {
                  text: "GDPR",
                  value: "code",
                  class: "text-left black--text text-h6",
              },
              {
                  text: "Fullname",
                  value: "fullname",
                  class: "text-left black--text text-h6",
              },
              {
                  text: "Email",
                  value: "id",
                  class: "text-left black--text text-h6",
              },
              {
                  text: "Role",
                  value: "role",
                  class: "text-left black--text text-h6",
              },
          ],
        };
    },
    computed: {
        users: function (): UserViewModule[] {
            return storeHelper.userStore.users;
        },
    },
    created: async function () {
        await storeHelper.userStore.loadUsers();
        await storeHelper.specializationStore.loadSpecializations();
    },
});
</script>
