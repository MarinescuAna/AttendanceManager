
<template>
  <v-layout column>
    <!-- The tabs menu -->
    <v-tabs
      v-model="currentTab"
      background-color="transparent"
      color="black"
      centered
    >
      <v-tab> Create user</v-tab>
      <v-tab> Upload users</v-tab>
    </v-tabs>

    <v-tabs-items v-model="currentTab" class="pa-3 remove-background-color">
        <v-tab-item>
          <CreateUserComponent />
        </v-tab-item>
        <v-tab-item>
          <UploadUserComponent />
        </v-tab-item>
    </v-tabs-items>
  </v-layout>
</template>

<style lang="css" scoped>
.remove-background-color {
  background-color: transparent;
}
</style>

  <script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import CreateUserComponent from "@/components/user/CreateUserComponent.vue";
import UploadUserComponent from "@/components/user/UploadUserComponent.vue";

export default Vue.extend({
  name: "CreateUserView",
  components: {CreateUserComponent, UploadUserComponent},
  data() {
    return {
      currentTab: 1,
    };
  },
  async created() {
    await storeHelper.departmentStore.loadDepartments();
    await storeHelper.specializationStore.loadSpecializations();
  },
});
</script>
    