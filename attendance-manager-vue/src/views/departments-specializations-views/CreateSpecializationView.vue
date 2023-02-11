<template>
  <v-container>
    <v-card class="orange lighten-2">
      <v-card-title class="pa-7">
        <h2>Create new specialization</h2>
        <v-spacer></v-spacer>
        <router-link :to="{ name: 'department' }" tag="button"
          ><v-icon>mdi-close</v-icon></router-link
        >
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
                counter
                maxlength="128"
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
import { DepartmentViewModel } from "@/modules/department";
import { SpecializationInsertModule } from "@/modules/specialization";
import storeHelper from "@/store/store-helper";

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
      // Specialization name
      name: "",
      // The department id of the specialization
      department: "",
    };
  },
  computed: {
    // Departments list to load them in the v-selector
    departments(): DepartmentViewModel[] {
      return storeHelper.departmentStore.departments;
    },
  },
  methods: {
    /**
     * Use this method for adding a new specialization
     */
    async addSpecialization() {
      const response = await StoreHelper.specializationStore.addSpecialization({
        name: this.name,
        departmentId: Number.parseInt(this.department),
      } as SpecializationInsertModule);

      if (response.isSuccess) {
        this.$router.currentRoute.meta?.onBack();
      } else {
        window.alert(response.error);
      }
    },
  },
});
</script>
    