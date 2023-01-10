<template>
  <v-container>
    <v-card class="orange lighten-2">
      <v-card-title class="pa-7">
        <h2>Create new course</h2>
        <v-spacer></v-spacer>
        <router-link to="/courses" tag="button"
          ><v-icon>mdi-close</v-icon></router-link
        >
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(addCourse)">
            <validation-provider
              name="name"
              v-slot="{ errors }"
              rules="required"
            >
              <v-text-field
                v-model="name"
                type="text"
                label="Course name"
                prepend-icon="mdi-pencil"
                :error-messages="errors"
                required
                class="pa-6"
              />
            </validation-provider>
            <validation-provider
              rules="required"
              name="selectedDepartment"
              v-slot="{ errors }"
            >
              <v-select
                :items="departments"
                label="Department"
                :error-messages="errors"
                prepend-icon="mdi-folder-open"
                @change="onFillSpecializations"
                class="pa-6"
                item-text="name"
                item-value="id"
                required
              ></v-select>
            </validation-provider>
            <validation-provider
              rules="required"
              name="selectedSpecializations"
              v-slot="{ errors }"
            >
              <v-select
                :items="specializations"
                label="Specializations"
                :error-messages="errors"
                v-model="selectedSpecialization"
                prepend-icon="mdi-file"
                class="pa-6"
                item-text="name"
                item-value="id"
                :disabled="specializations.length == 0"
                return-object
                required
              ></v-select>
            </validation-provider>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addCourse"
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
import { SpecializationModule } from "@/modules/organization/specializations";
import { DepartmentModule } from "@/modules/organization/departments";
import storeHelper from "@/store/store-helper";
import { CreateCourseModule } from "@/modules/course";

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
      // Course name
      name: "",
      // All the specializations
      specializations: [] as SpecializationModule[],
      // Selected specializations
      selectedSpecialization: Object as () => SpecializationModule,
    };
  },
  created(){
    if(storeHelper.organizationStore.organisationsExists==0){
        storeHelper.organizationStore.loadOrganizations();
      }
  },
  computed: {
    /**
     * Get departments to fill the v-selector
     */
    departments(): DepartmentModule[] {
      this.onResetSpecializations();
      return storeHelper.organizationStore.departments;
    },
  },
  methods: {
    /**
     * Use this method for adding a new specialization
     * Success: reset the form and reload the treeview
     * Error: display the message
     */
    async addCourse() {
      const response = await StoreHelper.courseStore.addCourse({
        name: this.name,
        specializationId: this.selectedSpecialization.id,
        specializationName: this.selectedSpecialization.name
      } as CreateCourseModule);

      if (response.isSuccess) {
        this.$router.currentRoute.meta?.onBack();
      } else {
        window.alert(response.error);
      }
    },
    /**
     * Get the list with all specializations by department id
     * @param selectedDepartment
     */
    onFillSpecializations(selectedDepartment): void {
      console.log(storeHelper.organizationStore.organizations)
      console.log(storeHelper.organizationStore.specializations(selectedDepartment))
      this.specializations =
        storeHelper.organizationStore.specializations(selectedDepartment);
    },
    /**
     * Call this function whenever you reset the department selector
     */
    onResetSpecializations(): void{
      this.specializations = [];
      this.selectedSpecialization = Object as () => SpecializationModule
    }
  },
});
</script>
      