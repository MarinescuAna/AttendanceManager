<template>
  <v-layout column>
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
    <v-tabs-items class="mt-3 custom-remove-background" v-model="currentTab" touchless>
      <v-tab-item>
       <DepartmentSpecializationViewTab />
      </v-tab-item>
      <v-tab-item>
        <CreateDepartmentTab/>
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

export default Vue.extend({
  name: "DepartmentView",
  components: {
    DepartmentSpecializationViewTab,
    CreateDepartmentTab,
    CreateSpecializationTab
},
  data: function () {
    return {
      currentTab: 0,
    };
  },
  created: async function () {
    await storeHelper.departmentStore.loadDepartments();
  },
});
</script>