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
  <v-layout column v-else>
    <v-tabs
      v-model="currentTab"
      background-color="transparent"
      color="black"
      centered
      show-arrows
      align-with-title
    >
      <v-tab>View departments and specializations</v-tab>
      <v-tab>Add departments</v-tab>
      <v-tab>Add specializations</v-tab>
    </v-tabs>
    <v-tabs-items
      class="mt-3 custom-remove-background"
      v-model="currentTab"
      touchless
    >
      <v-tab-item>
        <DepartmentSpecializationViewTab />
      </v-tab-item>
      <v-tab-item>
        <CreateDepartmentTab />
      </v-tab-item>
      <v-tab-item>
        <CreateSpecializationTab />
      </v-tab-item>
    </v-tabs-items>
  </v-layout>
</template>

<style scoped>
.custom-remove-background {
  background-color: transparent;
}
</style>

<script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import DepartmentSpecializationViewTab from "@/components/department-specialization/DepartmentSpecializationViewTab.vue";
import CreateDepartmentTab from "@/components/department-specialization/CreateDepartmentTab.vue";
import CreateSpecializationTab from "@/components/department-specialization/CreateSpecializationTab.vue";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "DepartmentView",
  components: {
    DepartmentSpecializationViewTab,
    CreateDepartmentTab,
    CreateSpecializationTab,
  },
  data: function () {
    return {
      currentTab: 0,
      // Add a flag to track if data fetching is successful
      isFetchSuccessful: false,
      isLoading: true,
    };
  },
  created: async function () {
     // Fetch data with a timeout of 30 seconds
     const fetchDataWithTimeout = async () => {
      try {
        await storeHelper.departmentStore.loadDepartments();
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
});
</script>