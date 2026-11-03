namespace GameServer.Gateway.Tests;

public sealed class BowSkillsCatalogTests
{
    [Fact]
    public void BowSkillsResolveTheirProjectileDefinitionsAndSecondaryEffects()
    {
        var skills = new SkillCatalog();

        int splitArrow = skills.EffectRow("SplitArrow");
        Assert.True(splitArrow >= 0);
        var splitArrowProj = skills.ProjectileFor(splitArrow);
        Assert.NotNull(splitArrowProj);
        Assert.Equal("Metadata/Projectiles/SplitArrowDefault", splitArrowProj!.Value.Id);
        Assert.Equal(3260, splitArrowProj.Value.Speed);

        int iceShot = skills.EffectRow("IceShot");
        Assert.True(iceShot >= 0);
        var iceShotProj = skills.ProjectileFor(iceShot);
        Assert.NotNull(iceShotProj);
        Assert.Equal("Metadata/Projectiles/IceArrow", iceShotProj!.Value.Id);
        Assert.Equal(3260, iceShotProj.Value.Speed);

        var iceShotProfile = GatewayProjectileSkillProfiles.Resolve("IceShot", iceShotProj.Value.Id,
            name => name == "active_skill_base_area_of_effect_radius" ? 24 : 0, 3260, attack: true, arrows: true);
        Assert.NotNull(iceShotProfile.ContactArea);
        Assert.True(iceShotProfile.ContactArea!.ExcludeContactTarget);
        Assert.Equal(24, iceShotProfile.ContactArea.Radius);

        int galvanicArrow = skills.EffectRow("GalvanicArrow");
        Assert.True(galvanicArrow >= 0);
        var galvanicArrowProj = skills.ProjectileFor(galvanicArrow);
        Assert.NotNull(galvanicArrowProj);
        Assert.Equal("Metadata/Projectiles/ShrapnelShot", galvanicArrowProj!.Value.Id);
        Assert.Equal(3260, galvanicArrowProj.Value.Speed);

        int lightningArrow = skills.EffectRow("LightningArrow");
        Assert.True(lightningArrow >= 0);
        var lightningArrowProj = skills.ProjectileFor(lightningArrow);
        Assert.NotNull(lightningArrowProj);

        var lightningArrowProfile = GatewayProjectileSkillProfiles.Resolve("LightningArrow", lightningArrowProj!.Value.Id,
            name => name == "lightning_arrow_maximum_number_of_extra_targets" ? 3
                : name == "active_skill_base_area_of_effect_radius" ? 18 : 0, 3260, attack: true, arrows: true);
        Assert.NotNull(lightningArrowProfile.ContactTargets);
        Assert.Equal(3, lightningArrowProfile.ContactTargets!.Count);
        Assert.Equal(18, lightningArrowProfile.ContactTargets.Radius);
    }
}
