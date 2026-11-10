# Test Strategy

- There is no project EditMode or PlayMode test assembly under `Assets/`. The Test Framework package is installed. When the first gameplay rule (damage, scoring, stats, economy, or identity) is added, create an EditMode assembly and cover that rule with a test that calls it with its inputs. The test does not need a scene.
- A feature is done when its acceptance criteria pass, the required tests were run, and the docs that changed still match the code.
- The report says which tests ran, which editor flows were exercised, and what was not run.
- Never claim Unity Play Mode, a build, or console cleanliness unless that run happened.
- A passing EditMode suite is not evidence that a scene flow works. Say which path was not played.
- Do not add a PlayMode suite or a UI automation framework as a side effect of setup. The core loop needs one manual play of the path before any broader test harness.
