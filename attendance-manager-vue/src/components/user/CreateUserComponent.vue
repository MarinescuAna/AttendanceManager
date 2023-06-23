
<template>
  <v-container>
    <v-row justify="center">
      <v-card width="50%">
        <v-card-title class="pa-7">
          <h2>Create single user</h2>
        </v-card-title>
        <v-card-text>
          <validation-observer
            ref="observer"
            v-slot="{ handleSubmit, invalid }"
          >
            <v-form @submit.prevent="handleSubmit(onSubmitAsync)">
              <validation-provider
                :rules="rules.required"
                name="fullname"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Fullname"
                  v-model="fullname"
                  counter
                  color="black"
                  maxlength="128"
                  prepend-icon="mdi-account"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <validation-provider
                :rules="rules.email"
                name="email"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Email"
                  v-model="email"
                  counter
                  color="black"
                  maxlength="254"
                  prepend-icon="mdi-email"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <v-select
                :items="departments"
                label="Department"
                prepend-icon="mdi-folder-open"
                @change="onFillSpecializations"
                class="pa-6"
                item-text="name"
                color="black"
                item-value="id"
                ref="departmentRef"
                required
              ></v-select>
              <div class="ml-6">
                <label class=""><v-icon>mdi-pencil</v-icon> Role</label>
                <v-radio-group
                  v-model="role"
                  color="black"
                  class="ml-6"
                  @change="onResetSpecializationSelector"
                >
                  <v-row>
                    <v-radio class="ma-4" color="black" label="Student" :value="1"></v-radio>
                    <v-radio class="ma-4" color="black" label="Teacher" :value="2"></v-radio>
                  </v-row>
                </v-radio-group>
              </div>
              <v-select
                :items="specializations"
                label="Specializations"
                v-model="selectedSpecializations"
                prepend-icon="mdi-file"
                class="pa-6"
                color="black"
                item-text="name"
                item-value="id"
                :disabled="specializations.length == 0"
                attach
                chips
                deletable-chips
                multiple
                required
                v-if="role == 2"
              ></v-select>
              <v-select
                :items="specializations"
                label="Specializations"
                v-model="selectedSpecialization"
                prepend-icon="mdi-file"
                class="pa-6"
                color="black"
                item-text="name"
                item-value="id"
                :disabled="specializations.length == 0"
                required
                v-else
              ></v-select>
              <v-row justify="center">
                <v-col cols="6">
                  <validation-provider
                    :rules="rules.required"
                    name="GDPR"
                    v-slot="{ errors }"
                  >
                    <v-text-field
                      label="GDPR"
                      v-model="code"
                      counter
                      color="black"
                      maxlength="32"
                      prepend-icon="mdi-account"
                      :error-messages="errors"
                      class="pa-6"
                      required
                    />
                  </validation-provider>
                </v-col>
                <v-col cols="6">
                  <v-select
                    :items="years"
                    :label="role == 2 ? 'Employment year' : 'Enrollment year'"
                    v-model="year"
                    required
                    color="black"
                    prepend-icon="mdi-school"
                    class="pa-6"
                  ></v-select>
                </v-col>
              </v-row>
              <v-row justify="center" class="pa-8">
                <v-btn
                  width="50%"
                  @click="onSubmitAsync"
                  class="dark_button white--text"
                  :disabled="
                    invalid ||
                    (selectedSpecializations.length === 0 && role === 2) ||
                    (selectedSpecialization === 0 && role === 1) ||
                    year === 0
                  "
                  large
                  >Submit</v-btn
                >
              </v-row>
            </v-form>
          </validation-observer>
        </v-card-text>
      </v-card></v-row
    >
  </v-container>
</template>
    
    <script lang="ts">
import Vue from "vue";
import { rules } from "@/plugins/vee-validate";
import storeHelper from "@/store/store-helper";
import { Role } from "@/shared/enums";
import { Toastification } from "@/plugins/vue-toastification";
import {
  DepartmentViewModule,
  SpecializationViewModule,
} from "@/modules/view-modules";
import { InsertUserParameters } from "@/modules/commands-parameters";

export default Vue.extend({
  name: "CreateUserComponent",
  data() {
    return {
      rules,
      email: "",
      fullname: "",
      code: "",
      //user's role; Default value is 1 => student
      role: 1,
      year: 0,
      // All the specializations
      specializations: [] as SpecializationViewModule[],
      // Selected specializations
      selectedSpecializations: [] as number[],
      // Selected specialization if just one specialization can be selected
      selectedSpecialization: 0,
    };
  },
  computed: {
    /**
     * Get all the years between the current year and 1950
     */
    years(): string[] {
      return Array.from(Array(new Date().getFullYear() - 1949), (_, i) =>
        (new Date().getFullYear() - i).toString()
      );
    },
    departments(): DepartmentViewModule[] {
      return storeHelper.departmentStore.departments;
    },
  },
  methods: {
    onSubmitAsync: async function(): Promise<void> {
      let specializationIds: number[] = [];
      if (this.role == Role.Teacher) {
        specializationIds = this.selectedSpecializations;
      } else {
        specializationIds.push(this.selectedSpecialization);
      }

      const department = storeHelper.departmentStore.departments.find(
        (d) => d.id == this.specializations[0].departmentId
      );

      const response = await storeHelper.userStore.addUserAsync(
        {
          fullname: this.fullname,
          code: this.code,
          email: this.email,
          role: this.role.toString(),
          year: this.year,
          specializationIds: specializationIds,
        } as InsertUserParameters,
        department!
      );

      if (response) {
        Toastification.info(
          "The user was created and he will be inform via email regarding his credentials."
        );
        this.$router.push({ name: "users" });
      }
    },
    onFillSpecializations: function(selectedDepartment: number): void {
      this.specializations =
        storeHelper.specializationStore.specializations.filter(
          (specialization) => specialization.departmentId == selectedDepartment
        );
      this.selectedSpecializations = [];
    },
    onResetSpecializationSelector: function(): void {
      this.selectedSpecializations = [];
    },
  },
});
</script>
      