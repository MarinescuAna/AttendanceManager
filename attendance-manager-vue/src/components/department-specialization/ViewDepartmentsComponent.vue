<template>
  <v-container>
    <v-card class="orange lighten-3" min-width="50%" min-height="50%">
      <v-col>
        <v-treeview :items="departments" open-all multiple-active> </v-treeview>
      </v-col>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { DepartmentViewDTO } from "@/store/modules/organization";
import Vue from "vue";
import StoreHelper from "@/store/store-helper";

export default Vue.extend({
  data() {
    return {
      // List with all the departments
      departments: [] as DepartmentViewDTO[],
    };
  },
  /**
   * Load all the departments
   */
  async created() {
    this.departments = await StoreHelper.organizationStore.loadDepartments();
  },
});
</script>