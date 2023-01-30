<template>
  <v-container>
    <v-row justify="center">
      <v-btn-toggle>
        <v-btn class="blue-grey lighten-4" :to="{name:'new-department'}" 
          >Add new department</v-btn
        >
        <v-btn
          class="blue-grey lighten-4"
          :to="{name:'new-specialization'}" 
          >Add new specialization</v-btn
        >
      </v-btn-toggle>
    </v-row>
    <v-row justify="center">
      <v-col cols="6">
        <router-view></router-view>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="12">
        <ManagementTableComponent :dataSource="departments" :headers="headers" :title="'Departments'" :type="type" />
      </v-col>
    </v-row>
  </v-container>
</template>
<script lang="ts">
import Vue from "vue";
import ManagementTableComponent from "@/components/shared-components/ManagementTableComponent.vue";
import storeHelper from "@/store/store-helper";
import { DepartmentsHeader } from "@/components/shared-components/Headers";
import { ManagementDataType } from "@/shared/enums";
import { DepartmentViewModel } from "@/modules/department";

export default Vue.extend({
  components: {
    ManagementTableComponent
  },
  computed:{
    departments(): DepartmentViewModel[]{
      return storeHelper.departmentStore.departments;
    }
  },
  data(){
    return {
      headers: DepartmentsHeader,
      type: ManagementDataType.Department
    };
  },
  async created(){
    await storeHelper.departmentStore.loadDepartments();
  }
});
</script>