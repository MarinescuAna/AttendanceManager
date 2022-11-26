<template>
  <v-container>
    <v-row align="center">
      <v-col cols="6">
        <CreateDepartmentComponent />
      </v-col>
      <v-col cols="6"> 
        <CreateSpecializationComponent />
      </v-col>
    </v-row>
    <v-row align="center">
      <v-col cols="12">
        <ViewDepartmentsComponent />
      </v-col>
    </v-row>
  </v-container>
</template>
<script lang="ts">
import Vue from "vue";
import ViewDepartmentsComponent from "@/components/department-specialization/ViewDepartmentsComponent.vue";
import CreateDepartmentComponent from "@/components/department-specialization/CreateDepartmentComponent.vue";
import CreateSpecializationComponent from "@/components/department-specialization/CreateSpecializationComponent.vue";

export default Vue.extend({
  components: {
    ViewDepartmentsComponent,
    CreateDepartmentComponent,
    CreateSpecializationComponent
  },
});
</script>