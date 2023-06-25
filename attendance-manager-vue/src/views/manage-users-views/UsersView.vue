<template>
  <div v-if="isLoading">
    <v-layout justify-center>
      <v-progress-circular
        :size="100"
        :width="8"
        color="black"
        indeterminate
      ></v-progress-circular>
    </v-layout>
  </div>
  <div v-else-if="users.length != 0">
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
          <v-toolbar-title class="font-weight-bold">Users</v-toolbar-title>
        </v-toolbar>
      </template>
      <template v-slot:[`item.updated`]="{ item }">
        {{ getRelativeTime(item.updated) }}
      </template>
      <template v-slot:expanded-item="{ headers, item }">
        <td :colspan="headers.length">
          <UserInfoCardComponent :item="item" />
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
import { Toastification } from "@/plugins/vue-toastification";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import moment from "moment";

export default Vue.extend({
  name: "UsersView",
  components: { UserInfoCardComponent, MessageComponent },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      isLoading: true,
      // Add a flag to track if data fetching is successful
      isFetchSuccessful: false,
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
        {
          text: "Last update",
          value: "updated",
          class: "text-left black--text text-h6",
        }
      ],
    };
  },
  computed: {
    users: function (): UserViewModule[] {
      return storeHelper.userStore.users;
    },
  },
  created: async function () {

    // Fetch data with a timeout of 30 seconds
    const fetchDataWithTimeout = async () => {
      try {
        await storeHelper.userStore.loadUsersAsync();
        await storeHelper.specializationStore.loadSpecializationsAsync();
        this.isFetchSuccessful = true;
      } catch (error) {
        Toastification.simpleError("An error occurred during data fetching.");
      } finally {
        this.isLoading = false;
      }
    };

    // Start fetching data
    fetchDataWithTimeout();

    // Set a timeout to hide the loader if data is not fetched
    setTimeout(() => {
      if (!this.isFetchSuccessful) {
        this.isLoading = false;
        Toastification.simpleError("Data fetching timeout");
      }
    }, 30000);
  },
  methods:{
    getRelativeTime(updateOn: string) {
      return moment(new Date(updateOn)).fromNow();
    },
  }
});
</script>
