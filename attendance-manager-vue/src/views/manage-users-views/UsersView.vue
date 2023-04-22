<template>
    <ManagementTableComponent
      :dataSource="users"
      :headers="headers"
      title="Users"
      :type="type"
      displayMessage="There is no student added."
    />
</template>

<script lang="ts">
import Vue from "vue";
import ManagementTableComponent from "@/components/shared-components/ManagementTableComponent.vue";
import { UserViewModule } from "@/modules/user";
import { UserHeader } from "@/components/shared-components/Headers";
import { ManagementDataType } from "@/shared/enums";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  name: "UsersView",
  components: {
    ManagementTableComponent,
  },
  data: function () {
    return {
      headers: UserHeader,
      type: ManagementDataType.Users,
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
