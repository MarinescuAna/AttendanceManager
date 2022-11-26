<template>
  <v-container>
    <v-card min-width="50%" class="orange lighten-3">
      <v-card-title class="pa-7">
        <h2>Create new department</h2>
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(addSpecialization)">
            <validation-provider
              name="name"
              v-slot="{ errors }"
              rules="required"
            >
              <v-text-field
                v-model="name"
                type="text"
                label="Specialization name"
                prepend-icon="mdi-pencil"
                :error-messages="errors"
                required
                class="pa-6"
              />
            </validation-provider>
            <validation-provider
              name="department"
              v-slot="{ errors }"
              rules="required"
            >
              <v-select
                :items="departments"
                label="Departments"
                item-text="name"
                item-value="id"
                v-model="department"
                required
                :error-messages="errors"
                class="pa-6"
              ></v-select>
            </validation-provider>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addSpecialization"
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
import { DepartmentModule } from "@/shared/modules";
import { SpecializationCreateDTO } from "@/store/modules/organization";

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
      name: "",
      // The department id of the specialization
      department: "",
      // Departments list
      departments: [] as DepartmentModule[],
    };
  },
  /**
   * Load all the departments
   */
  created() {
    this.departments = StoreHelper.organizationStore.departmentsOnly;
  },
  methods: {
    /**
     * Use this method for adding a new specialization
     */
    async addSpecialization() {
      const response = await StoreHelper.organizationStore.addSpecialization({
        name: this.name,
        departmentId: this.department,
      } as SpecializationCreateDTO);
      //TODO Handle exceptions
      //TODO Reload the component when a new Specialization was added
    },
  },
});
</script>
    