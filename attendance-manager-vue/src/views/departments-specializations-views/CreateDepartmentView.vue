<template>
  <v-container>
    <v-card class="orange lighten-2">
      <v-card-title class="pa-7">
        <h2>Create new department</h2>
         <v-spacer></v-spacer>
         <router-link :to="{name:'department'}" tag="button"><v-icon>mdi-close</v-icon></router-link> 
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(addDepartment)">
            <validation-provider
              name="department name"
              v-slot="{ errors }"
              :rules="rules.required"
            >
              <v-text-field
                v-model="department"
                type="text"
                label="Department name"
                prepend-icon="mdi-pencil"
                :error-messages="errors"
                required
                counter
                maxlength="128"
                class="pa-6"
              />
            </validation-provider>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addDepartment"
                :disabled="invalid"
                large
                class="blue-grey lighten-4"
                >Submit</v-btn
              >
            </v-row>
          </v-form>
        </validation-observer>
      </v-card-text>
    </v-card>
  </v-container>
</template>


<script lang="ts">
import Vue from "vue";
import { rules } from "@/plugins/vee-validate";
import StoreHelper from "@/store/store-helper";

export default Vue.extend({
  data() {
    return {
      rules,
      // Department name
      department: "",
    };
  },
  methods: {
    /**
     * Add new department in store and db
     * Success: reset the form and reload the treeview
     */
    async addDepartment() {
      const response = await StoreHelper.departmentStore.addDepartment(this.department);

      if (response) {
        this.$router.currentRoute.meta?.onBack();
      }
    },
  },
});
</script>
  