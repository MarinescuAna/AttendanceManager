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
              name="password"
              v-slot="{ errors }"
              rules="required"
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
import { extend } from "vee-validate";
import { required } from "vee-validate/dist/rules";
import StoreHelper from "@/store/store-helper";
import { ResponseModule } from "@/shared/modules";

/**
 * Validation for requied
 */
//TODO move this into a shared class
extend("required", {
  ...required,
  message: "{_field_} can not be empty",
});

export default Vue.extend({
  data() {
    return {
      // Department name
      department: "",
    };
  },
  methods: {
    /**
     * Add new department in store and db
     * Success: reset the form and reload the treeview
     * Error: display error
     */
    async addDepartment() {
      const response = (await StoreHelper.departmentStore.addDepartment(this.department)) as ResponseModule;

      if (response.isSuccess) {
        this.$router.currentRoute.meta?.onBack();
      } else {
        window.alert(response.error);
      }
    },
  },
});
</script>
  