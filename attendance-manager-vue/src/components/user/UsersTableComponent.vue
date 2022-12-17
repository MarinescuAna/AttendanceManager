<template>
  <v-data-table
    :headers="headers"
    :items="users"
    :expanded.sync="expanded"
    item-key="userId"
    dense
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
        <v-container class="ma-2">
          <v-row>
            <h2>{{ item.fullname }}({{ item.code }})</h2>
          </v-row>
          <v-row v-if="!item.accountConfirmed">
            <v-alert text type="warning"
              >This user didn't confirmed his account!</v-alert
            >
          </v-row>
          <v-container class="mt-2">
            <p><strong>Email: </strong>{{ item.email }}</p>
            <p><strong>Role: </strong>{{ item.role }}</p>
            <p><strong>Department: </strong>{{ item.departmentName }}</p>
            <p><strong>Enrollment year: </strong>{{ item.enrollmentYear }}</p>
            <p><strong>Specialization:</strong></p>
            <ul v-for="s in item.userSpecializations" :key="s.id">
              {{
                s.name
              }}
            </ul>
          </v-container>
        </v-container>
      </td>
    </template>
  </v-data-table>
</template>
<script lang="ts">
import { UserViewModule } from "@/modules/user";
import storeHelper from "@/store/store-helper";
import Vue from "vue";

export default Vue.extend({
  data() {
    return {
      // The headers list
      headers: [
        {
          text: "Code",
          value: "code",
        },
        {
          text: "Fullname",
          value: "fullname",
        },
        {
          text: "Email",
          value: "email",
        },
        {
          text: "Role",
          value: "role",
        },
      ],
      // Elements that are currently expanded
      expanded: [],
      // List with all the users
      users: [] as UserViewModule[],
    };
  },
  /**
   * Load all the users from API and add them also into the store
   */
  async created() {
    this.users = await storeHelper.userStore.loadUsers();
  },
});
</script>