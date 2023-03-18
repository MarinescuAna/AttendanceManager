<template>
  <v-container>
    <v-card class="orange lighten-3">
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
              :rules="rules.required"
            >
              <v-text-field
                v-model="name"
                type="text"
                label="Course name"
                counter
                maxlength="128"
                prepend-icon="mdi-pencil"
                :error-messages="errors"
                required
                class="pa-6"
              />
            </validation-provider>
            <v-select
              :items="specializations"
              label="Specializations"
              v-model="selectedSpecialization"
              prepend-icon="mdi-file"
              class="pa-6"
              item-text="name"
              item-value="id"
              :disabled="specializations.length == 0"
              required
            ></v-select>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addCourse"
                :disabled="invalid || selectedSpecialization === 0"
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
import storeHelper from "@/store/store-helper";
import { CreateCourseModule } from "@/modules/course";
import { SpecializationModule } from "@/modules/specialization";

export default Vue.extend({
  data() {
    return {
      rules,
      // Course name
      name: "",
      // Selected specializations
      selectedSpecialization: 0,
    };
  },
  computed: {
    // All the specializations
    specializations(): SpecializationModule[] {
      return storeHelper.userStore.currentUser.specializations;
    },
  },
  methods: {
    /**
     * Use this method for adding a new specialization
     * Success: reset the form and reload the treeview
     */
    async addCourse() {
      const response = await storeHelper.courseStore.addCourse({
        name: this.name,
        specializationId: this.selectedSpecialization,
        specializationName: this.specializations.find(
          (x) => x.id == this.selectedSpecialization
        )!.name,
      } as CreateCourseModule);

      if (response) {
        this.$router.currentRoute.meta?.onBack();
      }
    },
    /**
     * Call this function whenever you reset the department selector
     */
    onResetSpecializations(): void {
      this.specializations = [];
      this.selectedSpecialization = 0;
    },
  },
});
</script>
      