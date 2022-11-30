<template>
  <v-container>
    <v-row justify="center">
      <v-btn-toggle>
        <v-btn class="blue-grey lighten-4" @click="onCreateDepartmentComponent"
          >Add new department</v-btn
        >
        <v-btn
          class="blue-grey lighten-4"
          @click="onCreateSpecializationComponent"
          >Add new specialization</v-btn
        >
      </v-btn-toggle>
    </v-row>
    <v-row justify="center">
      <v-col cols="6">
        <CreateDepartmentComponent v-if="showDepartment" />
        <CreateSpecializationComponent v-if="showSpecialization" />
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="12">
        <ViewDepartmentsComponent/>
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
    CreateSpecializationComponent,
  },
  data() {
    return {
      // Boolean for displaying the create department component
      showDepartment: false,
      // Boolean for displaying the create specialization component
      showSpecialization: false,
    };
  },
  methods: {
    /**
     * Manage the way of displaying the CreateDepartmentComponent and close the CreateSpecializationComponent
     */
    onCreateDepartmentComponent(): void {
      this.showDepartment = !this.showDepartment;
      this.showSpecialization = false;
    },
    /**
     * Manage the way of displaying the CreateSpecializationComponent and close the CreateDepartmentComponent
     */
    onCreateSpecializationComponent(): void {
      this.showSpecialization = !this.showSpecialization;
      this.showDepartment = false;
    }
  },
});
</script>