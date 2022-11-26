<template>
  <v-container>
    <v-card min-width="50%" class="orange lighten-3">
      <v-card-title class="pa-7">
        <h2>Create new department</h2>
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
                class="pa-6"
              />
            </validation-provider>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addDepartment"
                :disabled="invalid"
                large
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
     * Use this method for adding a new department
     */
    async addDepartment() {
      const response = await StoreHelper.organizationStore.addDepartment(
        this.department
      );
      //TODO do something when a error occures
      //TODO do something when is success
    },
  },
});
</script>
  