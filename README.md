# Video

https://disk.yandex.ru/i/kPcIaz-itnAz8w

# Build

https://disk.yandex.ru/d/en_aniiqQBnHPQ

# English

## Project

This Unity project is a compact gameplay implementation for a technical assessment. Its purpose is to show a maintainable runtime architecture: explicit dependency ownership, testable gameplay rules, configurable content, and clear separation between Unity scene objects and domain-oriented logic.

## Architectural Approach

The project uses a feature-oriented structure with separate areas for combat, abilities, player logic, enemies, projectiles, arena/session flow, UI, loading, shared utilities, and composition.

Dependencies are managed through VContainer. The arena root registers shared services, and smaller feature lifetime scopes install player, enemy, session, and UI dependencies. Runtime binders connect validated Unity references to the registered services after the container is built. This keeps initialization explicit and avoids hidden scene lookups.

Responsibilities are split by use case. ScriptableObjects define tunable content such as stats, weapons, abilities, projectiles, enemy archetypes, and waves. Runtime classes execute behavior. Presenters coordinate state and UI-facing decisions. Factories own pooled runtime object creation. This approach was chosen because it gives the project clear extension points and testable boundaries without introducing architecture that is larger than the assignment.

## Design Decisions

* Dependency Injection: VContainer is used to make runtime dependencies explicit and keep object graph creation in the composition layer.
* Assembly Definitions: `Core`, `Combat`, `Gameplay`, `UI`, and `Composition` assemblies define dependency direction and reduce accidental coupling.
* Composition over Inheritance: Gameplay systems are built from focused components, executors, presenters, factories, and definitions instead of deep base-class hierarchies.
* MVP-style Presentation: UI and session-facing flows use presenters to keep Unity views thin and move state decisions, subscriptions, and coordination into testable classes.
* ScriptableObject Configuration: Gameplay content is data-driven so tuning can change independently of runtime behavior.
* Explicit Ownership: Factories own pooled projectile, enemy, and hit-effect creation so callers do not manage prefab setup or release rules.
* Runtime Validation: Scene references and content definitions are checked during composition to fail early with actionable errors.
* Testable Domain Logic: EditMode tests cover ability cooldowns and loadouts, damage pipelines, health behavior, enemy behavior, projectile impact resolution, pooling, session flow, runtime validation, and composition wiring.

# Русский

## Проект

Это компактная gameplay-реализация на Unity для технического задания. Ее цель — показать поддерживаемую runtime-архитектуру: явное владение зависимостями, тестируемые игровые правила, конфигурируемый контент и четкое разделение между Unity scene objects и domain-oriented логикой.

## Архитектурный подход

Проект использует feature-oriented структуру с отдельными областями для combat, abilities, player logic, enemies, projectiles, arena/session flow, UI, loading, shared utilities и composition.

Зависимости управляются через VContainer. Arena root регистрирует общие сервисы, а меньшие feature lifetime scopes устанавливают зависимости player, enemy, session и UI. Runtime binders подключают проверенные Unity references к зарегистрированным сервисам после сборки контейнера. Это делает инициализацию явной и избегает скрытых scene lookups.

Ответственности разделены по use case. ScriptableObjects определяют настраиваемый контент: stats, weapons, abilities, projectiles, enemy archetypes и waves. Runtime-классы выполняют поведение. Presenters координируют решения, связанные с состоянием и UI. Factories владеют созданием pooled runtime objects. Такой подход выбран потому, что дает проекту понятные точки расширения и тестируемые границы без архитектуры, которая больше самого задания.

## Архитектурные решения

* Dependency Injection: VContainer делает runtime-зависимости явными и удерживает создание object graph в composition layer.
* Assembly Definitions: сборки `Core`, `Combat`, `Gameplay`, `UI` и `Composition` задают направление зависимостей и снижают случайную связность.
* Composition over Inheritance: gameplay systems собираются из focused components, executors, presenters, factories и definitions вместо глубоких base-class hierarchies.
* MVP-style Presentation: UI и session-facing flow используют presenters, чтобы Unity views оставались тонкими, а решения по состоянию, подпискам и координации находились в тестируемых классах.
* ScriptableObject Configuration: gameplay content остается data-driven, поэтому tuning можно менять независимо от runtime behavior.
* Explicit Ownership: factories владеют созданием pooled projectiles, enemies и hit effects, поэтому вызывающий код не управляет prefab setup или release rules.
* Runtime Validation: scene references и content definitions проверяются во время композиции, чтобы ошибки проявлялись рано и были понятными.
* Testable Domain Logic: EditMode-тесты покрывают ability cooldowns и loadouts, damage pipelines, health behavior, enemy behavior, projectile impact resolution, pooling, session flow, runtime validation и composition wiring.
