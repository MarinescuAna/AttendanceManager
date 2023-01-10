<template>
  <v-card>
    <ManagementTableComponent :dataSource="users" :headers="headers" :title="'Users'" :type="type" />
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import ManagementTableComponent from "@/components/shared-components/ManagementTableComponent.vue";
import { UserViewModule } from "@/modules/user";
import { UserHeader } from "@/components/shared-components/Headers";
import { ManagementDataType } from "@/shared/enums";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  components: {
    ManagementTableComponent
  },
  data(){
    return {
      users: [] as UserViewModule[],
      headers: UserHeader,
      type: ManagementDataType.Users
    };
  },
  async created(){
    this.users = await storeHelper.userStore.loadUsers();
  }
});
</script>
