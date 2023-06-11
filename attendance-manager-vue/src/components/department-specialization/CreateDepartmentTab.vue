<template>
  <v-container>
    <v-card class="orange_background">
      <v-card-title class="pa-7">
        <h4>Create new department</h4>
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
                color="black"
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
                class="dark_button white--text"
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
import storeHelper from "@/store/store-helper";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "CreateDepartmentTab",
  data: function () {
    return {
      rules,
      department: "",
    };
  },
  methods: {
    addDepartment: async function (): Promise<void> {
      const response = await storeHelper.departmentStore.addDepartment({
        name: this.department,
      });

      if (response) {
        Toastification.success("The department was successfully added!");
        this.department = "";
      }
    },
  },
});
</script>
    