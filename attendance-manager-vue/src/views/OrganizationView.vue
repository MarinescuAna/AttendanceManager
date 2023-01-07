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
        <ViewDepartmentsComponent/>
      </v-col>
    </v-row>
  </v-container>
</template>
<script lang="ts">
import Vue from "vue";
import ViewDepartmentsComponent from "@/components/department-specialization/ViewDepartmentsComponent.vue";

export default Vue.extend({
  components: {
    ViewDepartmentsComponent
  }
});
</script>