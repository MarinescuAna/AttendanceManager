<template>
  <v-container>
    <v-card class="orange_background">
      <v-card-title class="pa-7">
        <h4>Create new specialization</h4>
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(addSpecialization)">
            <validation-provider
              name="specialization name"
              v-slot="{ errors }"
              :rules="rules.required"
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
                color="black"
                class="pa-6"
              />
            </validation-provider>
            <v-select
              :items="departments"
              label="Departments"
              item-text="name"
              item-value="id"
              v-model="department"
              required
              color="black"
              class="pa-6"
            ></v-select>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addSpecialization"
                :disabled="invalid || department === 0"
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
import storeHelper from "@/store/store-helper";
import { rules } from "@/plugins/vee-validate";
import { Toastification } from "@/plugins/vue-toastification";
import { DepartmentViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "CreateSpecializationTab",
  data() {
    return {
      rules,
      name: "",
      department: 0,
    };
  },
  computed: {
    departments(): DepartmentViewModule[] {
      return storeHelper.departmentStore.departments;
    },
  },
  methods: {
    async addSpecialization() {
      const response = await storeHelper.specializationStore.addSpecialization({
        name: this.name,
        departmentId: this.department,
      });

      if (response) {
        Toastification.success("The specialization was successfully added!");
        this.name = "";
        this.department = 0;
      }
    },
  },
});
</script>
      